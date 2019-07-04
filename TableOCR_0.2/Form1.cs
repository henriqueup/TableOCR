using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Tesseract;
using Leptonica;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TableOCR_0._2
{
    public partial class Form1 : Form
    {
        public FormScreenCapture fsc = null;
        const int borderLeft = 8;
        const int borderTop = 30;
        private string path;
        public Image imgTabelaOriginal;
        private Image imgTabelaNonBinarized;
        private Image imgTabelaNoLines;
        private Image imgTabelaLines;
        private System.Drawing.Graphics gfxTabela;
        private Size sizeOriginal;
        static CultureInfo ci = new CultureInfo("pt-BR");
        private int curHist;
        private int curUndoHist;
        private bool curImg;
        private bool original;
        private List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>> hist = new List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>>();
        private List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>> undoHist = new List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>>();
        private List<Tuple<Point, Point>> oldLines = new List<Tuple<Point, Point>>();
        public List<Tuple<Point, Point>> lines = new List<Tuple<Point, Point>>();
        public HashSet<Tuple<Point, Point>> removeLines = new HashSet<Tuple<Point, Point>>();
        public HashSet<Point> intersections = new HashSet<Point>();
        public List<List<Point>> intersectionMatrix = new List<List<Point>>();
        public List<List<Point>> tessIntersectionMatrix = new List<List<Point>>();
        public SortedSet<int> xValues = new SortedSet<int>();
        public SortedSet<int> yValues = new SortedSet<int>();
        List<List<Tuple<Point, Point, Point, Point>>> cells = new List<List<Tuple<Point, Point, Point, Point>>>();

        public Form1()
        {
            InitializeComponent();
            curHist = 0;
            curUndoHist = 0;
            sizeOriginal = this.Size;
            curImg = false;
            original = true;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            //this.Show();
        }

        private void Form1_Activate(object sender, EventArgs e)
        {
            //if (!fsc.IsDisposed)
            //{
            //    fsc.WindowState = FormWindowState.Minimized;
            //}
        }

        private void AddHist()
        {
            hist.Add(new Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>((Image)imgTabelaLines.Clone(), new List<Tuple<Point, Point>>(oldLines), new List<Tuple<Point, Point>>(lines)));
            curHist++;
        }

        private void AddUndoHist()
        {
            undoHist.Add(new Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>((Image)imgTabelaLines.Clone(), new List<Tuple<Point, Point>>(oldLines), new List<Tuple<Point, Point>>(lines)));
            curUndoHist++;
        }

        private List<Tuple<Point, Point>> FindLinesNear(Point p)
        {
            int x, mx, y, my, y1, y2, x1, x2;
            List<Tuple<Point, Point>> ret = new List<Tuple<Point, Point>>();
            if (checkBoxLinha.Checked)
            {
                x = -1;
                mx = 0;
                foreach (int val in xValues)
                {
                    int diff = p.X - val;
                    if (diff < 0)
                        diff *= -1;

                    if (diff < 3)
                    {
                        x = val;
                        break;
                    }
                    mx++;
                }
                if (x != -1)
                {
                    foreach (Tuple<Point, Point> t in oldLines)
                    {
                        if (t.Item1.X == x)
                        {
                            ret.Add(new Tuple<Point, Point>(t.Item1, t.Item2));
                        }
                    }
                }
                y = -1;
                my = 0;
                foreach (int val in yValues)
                {
                    int diff = p.Y - val;
                    if (diff < 0)
                        diff *= -1;

                    if (diff < 3)
                    {
                        y = val;
                        break;
                    }
                    my++;
                }
                if (y != -1)
                {
                    foreach (Tuple<Point, Point> t in oldLines)
                    {
                        if (t.Item1.Y == y)
                        {
                            ret.Add(new Tuple<Point, Point>(t.Item1, t.Item2));
                        }
                    }
                }
            }
            else
            {
                x = -1;
                mx = 0;
                foreach (int val in xValues)
                {
                    int diff = p.X - val;
                    if (diff < 0)
                        diff *= -1;

                    if (diff < 3)
                    {
                        x = val;
                        break;
                    }
                    mx++;
                }
                if (x != -1)
                {
                    my = 0;
                    y1 = -1;
                    y2 = -1;
                    foreach (int val in yValues)
                    {
                        int diff = p.Y - val;
                        if (diff <= 0)
                        {
                            y1 = my - 1;
                            y2 = my;
                            break;
                        }
                        my++;
                    }
                    if (y1 != -1 && y2 != -1)
                    {
                        try
                        {
                            ret.Add(new Tuple<Point, Point>(intersectionMatrix[mx][y1], intersectionMatrix[mx][y2]));
                        }
                        catch
                        {

                        }
                    }
                }
                y = -1;
                my = 0;
                foreach (int val in yValues)
                {
                    int diff = p.Y - val;
                    if (diff < 0)
                        diff *= -1;

                    if (diff < 3)
                    {
                        y = val;
                        break;
                    }
                    my++;
                }
                if (y != -1)
                {
                    mx = 0;
                    x1 = -1;
                    x2 = -1;
                    foreach (int val in xValues)
                    {
                        int diff = p.X - val;
                        if (diff <= 0)
                        {
                            x1 = mx - 1;
                            x2 = mx;
                            break;
                        }
                        mx++;
                    }
                    if (x1 != -1 && x2 != -1)
                    {
                        try
                        {
                            ret.Add(new Tuple<Point, Point>(intersectionMatrix[x1][my], intersectionMatrix[x2][my]));
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return ret;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                textBox1.Text = (Cursor.Position.X - this.Left - borderLeft) + ", " + (Cursor.Position.Y - this.Top - 80 - borderTop);
                Point clicked = new Point(Cursor.Position.X - this.Left - borderLeft, Cursor.Position.Y - this.Top - 80 - borderTop);
                List<Tuple<Point, Point>> nearLines = FindLinesNear(clicked);
                if (nearLines.Count > 0)
                {
                    Pen bluePen = new Pen(Color.Blue, 3);
                    Pen redPen = new Pen(Color.Red, 3);
                    foreach (Tuple<Point, Point> t in nearLines)
                    {
                        if (removeLines.Add(t))
                        {
                            gfxTabela.DrawLine(bluePen, t.Item1, t.Item2);
                        }
                        else
                        {
                            gfxTabela.DrawLine(redPen, t.Item1, t.Item2);
                            removeLines.Remove(t);
                        }
                    }
                    UpdatePainting();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (original)
            {
                e.Graphics.DrawImage(this.imgTabelaOriginal, 0, 80);
            }
            else
            {
                if (curImg)
                    e.Graphics.DrawImage(this.imgTabelaLines, 0, 80);
                else
                    e.Graphics.DrawImage(this.imgTabelaNoLines, 0, 80);
            }
        }

        public void UpdatePainting()
        {
            this.Invalidate();
            this.Paint += new PaintEventHandler(this.Form1_Paint);
        }

        public void UpdateGfx(int x1, int x2, int y1, int y2)
        {
            Pen redPen = new Pen(Color.Red, 2);
            gfxTabela.DrawLine(redPen, x1, y1, x2, y2);
        }

        public void LoadImage(Image<Gray, Byte> img)
        {
            //int threshold = 50;
            //imgTabelaOriginal = img.Convert<Gray, byte>().ThresholdBinary(new Gray(threshold), new Gray(255)).ToBitmap();
            imgTabelaOriginal = img.ToBitmap();
            int width = imgTabelaOriginal.Size.Width;
            int height = imgTabelaOriginal.Size.Height;

            if (height > width)
            {
                double ratio = (double)width / (double)height;
                int targetHeight = 1080;
                int targetWidth = Convert.ToInt32(ratio * targetHeight);
                imgTabelaOriginal = ResizeImage(imgTabelaOriginal, targetWidth, targetHeight);
            }
            else
            {
                double ratio = (double)height / (double)width;
                int targetWidth = 1920;
                int targetHeight = Convert.ToInt32(ratio * targetWidth);
                imgTabelaOriginal = ResizeImage(imgTabelaOriginal, targetWidth, targetHeight);
            }

            if (imgTabelaOriginal.Size.Width > sizeOriginal.Width)
            {
                this.Width = imgTabelaOriginal.Size.Width + 20;
            }
            else
            {
                this.Width = sizeOriginal.Width;
            }
            if (imgTabelaOriginal.Size.Height > sizeOriginal.Height)
            {
                this.Height = imgTabelaOriginal.Size.Height + 123;
            }
            else
            {
                this.Height = sizeOriginal.Height;
            }
            UpdatePainting();
            imgTabelaNonBinarized = imgTabelaOriginal;
        }

        private void buttonArquivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecione a Imagem";
            ofd.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg";
            //ofd.InitialDirectory = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                LoadImage(new Image<Gray, Byte>(ofd.FileName));
                //LoadImage(Image.FromFile(path));
            }
        }

        private void DrawTessIntersections()
        {
            Pen pen = new Pen(Color.Green, 4);
            foreach (List<Point> line in tessIntersectionMatrix)
            {
                foreach (Point p in line)
                {
                    gfxTabela.DrawLine(pen, p.X - 2, p.Y, p.X + 2, p.Y);
                }
            }
        }

        private void DrawIntersections()
        {
            Pen pen = new Pen(Color.Yellow, 4);
            const string txtPath1 = @"C:\Users\Henrique\Documents\Visual Studio 2017\Projetos\TableOCR\out1.txt";
            const string txtPath2 = @"C:\Users\Henrique\Documents\Visual Studio 2017\Projetos\TableOCR\out2.txt";
            FileStream fs1 = File.Create(txtPath1);
            FileStream fs2 = File.Create(txtPath2);
            foreach (Point p in intersections)
            {
                Byte[] info = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes(p.X + ", " + p.Y);
                fs1.Write(info, 0, info.Length);
                Byte[] emptyLine = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes(Environment.NewLine);
                fs1.Write(emptyLine, 0, emptyLine.Length);
                gfxTabela.DrawLine(pen, p.X - 2, p.Y, p.X + 2, p.Y);
            }
            foreach (List<Point> line in intersectionMatrix)
            {
                foreach (Point p in line)
                {
                    Byte[] info = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes("(" + p.X + ", " + p.Y + ")" + ", ");
                    fs2.Write(info, 0, info.Length);
                }
                Byte[] emptyLine = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes(Environment.NewLine);
                fs2.Write(emptyLine, 0, emptyLine.Length);
            }
            fs1.Close();
            fs2.Close();
        }

        private void PrintIntersectionMatrix()
        {
            foreach (List<Point> l in intersectionMatrix)
            {
                foreach (Point p in l)
                {
                    Debug.Write("(" + p.X + ", " + p.Y + ")" + " ");
                }
                Debug.Write(Environment.NewLine);
            }
        }

        private void PrintTessIntersectionMatrix()
        {
            foreach (List<Point> l in tessIntersectionMatrix)
            {
                foreach (Point p in l)
                {
                    Debug.Write("(" + p.X + ", " + p.Y + ")" + " ");
                }
                Debug.Write(Environment.NewLine);
            }
        }

        private void GetIntersectionMatrix()
        {
            IEnumerable<Point> sortedIntX = intersections.OrderBy(p => p.X);    //otimizavel? lambda mais complexo
            IEnumerable<Point> sortedIntersections = sortedIntX.OrderBy(p => p.Y);
            foreach (Point p in sortedIntersections)
            {
                xValues.Add(p.X);
                yValues.Add(p.Y);
            }

            foreach (int x in xValues)
            {
                intersectionMatrix.Add(new List<Point>());
                //tessIntersectionMatrix.Add(new List<Point>());
                foreach (int y in yValues)
                {
                    intersectionMatrix.Last<List<Point>>().Add(new Point(x, y));
                    //tessIntersectionMatrix.Last<List<Point>>().Add(new Point(x, y));
                    //Debug.Print(x + ", " + y);
                }
            }

            int i, j;
            for (i = 0; i < intersectionMatrix.Count - 1; i++)
            {
                for (j = 0; j < intersectionMatrix[i].Count - 1; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        lines.Add(new Tuple<Point, Point>(intersectionMatrix[i][j], intersectionMatrix[i][j + 1]));
                        lines.Add(new Tuple<Point, Point>(intersectionMatrix[i][j], intersectionMatrix[i + 1][j]));
                    }
                    else if (i % 2 != 0 && j % 2 != 0)
                    {
                        lines.Add(new Tuple<Point, Point>(intersectionMatrix[i - 1][j], intersectionMatrix[i][j]));
                        lines.Add(new Tuple<Point, Point>(intersectionMatrix[i][j - 1], intersectionMatrix[i][j]));
                    }
                }
            }
        }

        public void GetIntersections()
        {
            int i, j;
            double m_l1, m_l2;
            Point intersect;
            for (i = 0; i < oldLines.Count; i++)
            {
                for (j = 0; j < oldLines.Count; j++)
                {
                    if (i != j)
                    {
                        if ((oldLines[i].Item2.X - oldLines[i].Item1.X) != 0)
                            m_l1 = (oldLines[i].Item2.Y - oldLines[i].Item1.Y) / (oldLines[i].Item2.X - oldLines[i].Item1.X);
                        else
                            m_l1 = -1;
                        if ((oldLines[j].Item2.X - oldLines[j].Item1.X) != 0)
                            m_l2 = (oldLines[j].Item2.Y - oldLines[j].Item1.Y) / (oldLines[j].Item2.X - oldLines[j].Item1.X);
                        else
                            m_l2 = -1;

                        if (m_l1 != m_l2)
                        {
                            intersect = new Point();
                            if (m_l1 == 0)
                            {
                                intersect.X = oldLines[j].Item1.X;
                                intersect.Y = oldLines[i].Item1.Y;
                            }
                            else
                            {
                                intersect.X = oldLines[i].Item1.X;
                                intersect.Y = oldLines[j].Item1.Y;
                            }
                            intersections.Add(intersect);
                        }
                    }
                }
            }
            GetIntersectionMatrix();
            DrawIntersections();
        }

        private void buttonLinhas_Click(object sender, EventArgs e)
        {
            curImg = !curImg;
            UpdatePainting();
        }

        private void buttonFiltro_Click(object sender, EventArgs e)
        {
            if (fsc != null)
            {
                fsc.Close();
                fsc.Dispose();
            }
            

            Mat src = GetMatFromSDImage(imgTabelaOriginal);
            Mat dst = GetMatFromSDImage(imgTabelaOriginal);

            //int t1 = Int32.Parse(textBoxT1.Text);
            //int t2 = Int32.Parse(textBoxT2.Text);

            CvInvoke.Canny(src, dst, 200, 300, 3);
            src.Dispose();

            int tolerancia;
            if (Int32.TryParse(textBoxTolerancia.Text, out tolerancia))
            {
                lines.Clear();
                oldLines.Clear();
                intersections.Clear();
                intersectionMatrix.Clear();
                tessIntersectionMatrix.Clear();
                removeLines.Clear();
                xValues.Clear();
                yValues.Clear();

                var linhas = new VectorOfPointF();
                CvInvoke.HoughLines(dst, linhas, 1, Math.PI / 180, tolerancia, 0, 0);

                imgTabelaNoLines = dst.ToImage<Bgr, Byte>().ToBitmap();
                imgTabelaLines = dst.ToImage<Bgr, Byte>().ToBitmap();
                gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
                original = false;
                //UpdatePainting();

                for (int i = 0; i < linhas.Size; i++)
                {
                    float rho = linhas[i].X, theta = linhas[i].Y;
                    Point pt1 = new Point(), pt2 = new Point();
                    double a = Math.Cos(theta), b = Math.Sin(theta);
                    double x0 = a * rho, y0 = b * rho;
                    pt1.X = (int)Math.Round(x0 + imgTabelaLines.Width * (-b));
                    pt1.Y = (int)Math.Round(y0 + imgTabelaLines.Height * (a));
                    pt2.X = (int)Math.Round(x0 - imgTabelaLines.Width * (-b));
                    pt2.Y = (int)Math.Round(y0 - imgTabelaLines.Height * (a));
                    oldLines.Add(new Tuple<Point, Point>(pt1, pt2));
                    UpdateGfx(pt1.X, pt2.X, pt1.Y, pt2.Y);
                }
                GetIntersections();
                UpdatePainting();
                AddHist();
            }
            else
            {
                MessageBox.Show("Valor de tolerância invalido.");
            }
        }

        private void RemoveFromTessIntersectionMatrix(Point rmv)
        {
            int i = 0, j;
            foreach (List<Point> l in tessIntersectionMatrix)
            {
                j = 0;
                foreach (Point p in l)
                {
                    if (p.Equals(rmv))
                    {
                        tessIntersectionMatrix[i].RemoveAt(j);
                        return;
                    }
                    j++;
                }
                i++;
            }
        }

        private bool TessIntersectionMatrixContains(Point p_check)
        {
            foreach (List<Point> ls in tessIntersectionMatrix)
            {
                foreach (Point p in ls)
                {
                    if (p.Equals(p_check))
                    {
                        return true;
                    }
                    else if (p.Y > p_check.Y)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private void AddToTessIntersectionMatrix(Point p_add)
        {
            Debug.Print("(" + p_add.X + ", " + p_add.Y + ")");
            if (TessIntersectionMatrixContains(p_add))
            {
                return;
            }

            if (tessIntersectionMatrix.Count == 0)
            {
                tessIntersectionMatrix.Add(new List<Point>());
            }

            if (tessIntersectionMatrix.Last().Count > 0 && p_add.Y > tessIntersectionMatrix.Last()[0].Y)
            {
                tessIntersectionMatrix.Add(new List<Point>());
            }

            for (int j = 0; j < tessIntersectionMatrix.Count; j++)
            {
                if (tessIntersectionMatrix[j].Count == 0)
                {
                    tessIntersectionMatrix[j].Add(p_add);
                    return;
                }
                else
                {
                    if (tessIntersectionMatrix[j][0].Y == p_add.Y)
                    {
                        int i = 0;
                        foreach (Point p in tessIntersectionMatrix[j])
                        {
                            if (p.X > p_add.X)
                            {
                                tessIntersectionMatrix[j].Insert(i, p_add);
                                return;
                            }
                            i++;
                        }
                        tessIntersectionMatrix[j].Add(p_add);
                        return;
                    }
                    else if (tessIntersectionMatrix[j][0].Y > p_add.Y)
                    {
                        tessIntersectionMatrix.Insert(j, new List<Point>());
                        tessIntersectionMatrix[j].Add(p_add);
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private bool PointWasRemoved(Point p_check)
        {
            foreach (Tuple<Point, Point> t in removeLines)
            {
                if (t.Item1.Equals(p_check))
                {
                    return true;
                }
                else if (t.Item2.Equals(p_check))
                {
                    return true;
                }
            }
            return false;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            //if (removeLines.Count != 0)
            //{
                imgTabelaLines = (Image)imgTabelaNoLines.Clone();
                gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
                if (checkBoxLinha.Checked)
                {
                    foreach (Tuple<Point, Point> t in removeLines)
                    {
                        oldLines.Remove(t);
                        //RemoveFromTessIntersectionMatrix(t.Item1);
                        //RemoveFromTessIntersectionMatrix(t.Item2);
                    }
                    foreach (Tuple<Point, Point> t in oldLines)
                    {
                        UpdateGfx(t.Item1.X, t.Item2.X, t.Item1.Y, t.Item2.Y);
                        AddToTessIntersectionMatrix(t.Item1);
                        AddToTessIntersectionMatrix(t.Item2);
                    }
                }
                else
                {
                    foreach (Tuple<Point, Point> t in removeLines)
                    {
                        lines.Remove(t);
                        //RemoveFromTessIntersectionMatrix(t.Item1);
                        //RemoveFromTessIntersectionMatrix(t.Item2);
                    }
                    foreach (Tuple<Point, Point> t in lines)
                    {
                        UpdateGfx(t.Item1.X, t.Item2.X, t.Item1.Y, t.Item2.Y);
                        if (!PointWasRemoved(t.Item1))
                        {
                            AddToTessIntersectionMatrix(t.Item1);
                        }
                        if (!PointWasRemoved(t.Item2))
                        {
                            AddToTessIntersectionMatrix(t.Item2);
                        }
                    }
                }
                removeLines.Clear();

                DrawIntersections();
                DrawTessIntersections();
                UpdatePainting();
                AddHist();
                undoHist.Clear();
                curUndoHist = 0;
            //}
            //PrintIntersectionMatrix();
            Debug.Print("AQUI:");
            PrintTessIntersectionMatrix();
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (curHist != 0)
            {
                removeLines.Clear();
                AddUndoHist();
                imgTabelaLines = (Image)hist[curHist - 1].Item1.Clone();
                gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
                oldLines = new List<Tuple<Point, Point>>(hist[curHist - 1].Item2);
                lines = new List<Tuple<Point, Point>>(hist[curHist - 1].Item3);
                UpdatePainting();
                hist.RemoveAt(hist.Count - 1);
                curHist--;
            }
        }

        private void buttonRedo_Click(object sender, EventArgs e)
        {
            if (curUndoHist != 0)
            {
                removeLines.Clear();
                AddHist();
                imgTabelaLines = (Image)undoHist[curUndoHist - 1].Item1.Clone();
                gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
                oldLines = new List<Tuple<Point, Point>>(undoHist[curUndoHist - 1].Item2);
                lines = new List<Tuple<Point, Point>>(undoHist[curUndoHist - 1].Item3);
                UpdatePainting();
                undoHist.RemoveAt(undoHist.Count - 1);
                curUndoHist--;
            }
        }

        private Mat GetMatFromSDImage(System.Drawing.Image image)
        {
            int stride = 0;
            Bitmap bmp = new Bitmap(image);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            System.Drawing.Imaging.PixelFormat pf = bmp.PixelFormat;
            if (pf == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                stride = bmp.Width * 4;
            }
            else
            {
                stride = bmp.Width * 3;
            }

            Image<Bgra, byte> cvImage = new Image<Bgra, byte>(bmp.Width, bmp.Height, stride, (IntPtr)bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            return cvImage.Mat;
        }

        private bool[,] InvertMatrix(bool[,] matrix, int rows, int cols)
        {
            bool[,] ret = new bool[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    ret[i, j] = matrix[j, i];
                }
            }

            return ret;
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = System.Drawing.Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public VectorOfVectorOfPoint FindContours(Image<Gray, byte> image, ChainApproxMethod method = ChainApproxMethod.ChainApproxSimple, RetrType type = RetrType.List)
        {
            // Check that all parameters are valid.
            VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();

            if (method == Emgu.CV.CvEnum.ChainApproxMethod.ChainCode)
            {
                //throw new ColsaNotImplementedException("Chain Code not implemented, sorry try again later");
            }

            CvInvoke.FindContours(image, result, null, type, method);
            return result;
        }

        private void PrintCells()
        {
            foreach (List<Tuple<Point, Point, Point, Point>> ls in cells)
            {
                foreach (Tuple<Point, Point, Point, Point> t in ls)
                {
                    Debug.Write("[(" + t.Item1.X + ", " + t.Item1.Y + "), " + "(" + t.Item2.X + ", " + t.Item2.Y + "), " + "(" + t.Item3.X + ", " + t.Item3.Y + "), " + "(" + t.Item4.X + ", " + t.Item4.Y + ")], ");
                }
                Debug.Write(Environment.NewLine);
                Debug.Write(ls.Count);
            }
        }

        private Tuple<Point, Point> FindMatchingPoints(int startingLine, Point p1, Point p2)
        {
            Point p3 = new Point(-1, -1);
            Point p4 = new Point(-1, -1);

            for (int i = startingLine; i < tessIntersectionMatrix.Count; i++)
            {
                for (int j = 0; j < tessIntersectionMatrix[i].Count - 1; j++)
                {
                    Point t1 = tessIntersectionMatrix[i][j];
                    Point t2 = tessIntersectionMatrix[i][j + 1];
                    if (!t1.Equals(p1) && !t2.Equals(p2) && t1.Y == t2.Y && p1.X == t1.X && p2.X == t2.X && t1.Y > p1.Y) //talvez não precise de checar tanta coisa
                    {
                        p3 = t1;
                        p4 = t2;

                        return new Tuple<Point, Point>(p3, p4);
                    }
                }
            }

            return new Tuple<Point, Point>(p3, p4);
        }

        private void FillCellHoles()
        {
            for (int i = 1; i < cells.Count; i++)
            {
                if (cells[i].Count < cells[i - 1].Count)
                {
                    int firstCnt = cells[i].Count;
                    for (int j = 0; j < firstCnt; j++)
                    {
                        if (cells[i][j].Item1.X > cells[i - 1][j].Item2.X)
                        {
                            Tuple<Point, Point, Point, Point> t = new Tuple<Point, Point, Point, Point>(new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1));
                            cells[i].Insert(j, t);
                            cells[i].Insert(j, t);
                        }
                    }
                }
            }
        }

        private void ConstroiCells()
        {
            for (int i = 0; i < tessIntersectionMatrix.Count - 1; i++)
            {
                cells.Add(new List<Tuple<Point, Point, Point, Point>>());
                for (int j = 0; j < tessIntersectionMatrix[i].Count - 1; j++)
                {
                    Point p1 = tessIntersectionMatrix[i][j];
                    Point p2 = tessIntersectionMatrix[i][j + 1];

                    if (p1.Y == p2.Y)
                    {
                        Tuple<Point, Point> matchingPoints = FindMatchingPoints(i + 1, p1, p2);
                        Point p3 = matchingPoints.Item1;
                        Point p4 = matchingPoints.Item2;

                        //if (p3.X != -1 && p3.Y != -1 && p4.X != -1 && p4.Y != -1) //talvez não precise checar ambas as coordenadas de cada
                        //{
                            cells.Last().Add(new Tuple<Point, Point, Point, Point>(p1, p2, p3, p4));
                        //}
                    }
                }
            }

            FillCellHoles();
        }

        private void buttonTesseract_Click(object sender, EventArgs e)
        {
            //removeLines.Clear();
            //buttonRemove_Click(this, new EventArgs());
            //DrawTessIntersections();
            //UpdatePainting();

            int i, j;
            bool skip = false;
            //repensar maneira de criar estrutura cells
            ConstroiCells();
            PrintCells();

            string dataPath, language, inputFile;
            const string dir = @"R:\TabelaOCR\";
            dataPath = @"C:\Tesseract\tesseract-ocr\tessdata";
            language = "eng";
            OcrEngineMode oem = OcrEngineMode.DEFAULT;
            PageSegmentationMode psm = PageSegmentationMode.SINGLE_BLOCK;

            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            // Initialize tesseract-ocr
            if (!tessBaseAPI.Init(dataPath, language, oem))
            {
                throw new Exception("Could not initialize tesseract.");
            }

            // Set the Page Segmentation mode
            tessBaseAPI.SetPageSegMode(psm);

            i = 0;
            List<List<string>> matrix = new List<List<string>>();
            foreach (List<Tuple<Point, Point, Point, Point>> line in cells)
            {
                matrix.Add(new List<string>());

                j = 0;
                foreach (Tuple<Point, Point, Point, Point> cell in line)
                {
                    matrix[i].Add(".");
                    string myString = ".";
                    if (cell.Item1.X != -1 && cell.Item1.Y != -1 && cell.Item4.X != -1 && cell.Item4.Y != -1)
                    {
                        skip = false;
                        Rectangle roi = new Rectangle(cell.Item1.X + 4, cell.Item1.Y + 4, cell.Item4.X - cell.Item1.X - 4, cell.Item4.Y - cell.Item1.Y - 4);
                        if (roi.Height < 0)
                        {
                            roi.Height *= -1;
                        }
                        if (roi.Width < 0)
                        {
                            roi.Width *= -1;
                        }

                        Bitmap src = new Bitmap(imgTabelaOriginal);
                        Bitmap dst = new Bitmap(roi.Width, roi.Height);
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dst))
                        {
                            g.DrawImage(src, new Rectangle(0, 0, dst.Width, dst.Height), roi, GraphicsUnit.Pixel);
                        }
                        //dst = ResizeImage(dst, 1024, 720);
                        dst.Save(dir + "cell_" + i + "," + j + ".jpg");

                        VectorOfVectorOfPoint result = FindContours(new Image<Gray, Byte>(dst), ChainApproxMethod.ChainApproxSimple, RetrType.List);

                        if (result == null || !(result.Size > 0))
                        {
                            skip = true;
                            Debug.Print("skip");
                        }
                        src.Dispose();
                        dst.Dispose();

                        inputFile = dir + "cell_" + i + "," + j + ".jpg";
                        Pix pix = tessBaseAPI.SetImage(inputFile);
                        //File.Delete(dir + "cell" + i + ".png");

                        tessBaseAPI.Recognize();

                        ResultIterator resultIterator = tessBaseAPI.GetIterator();

                        StringBuilder stringBuilder = new StringBuilder();
                        PageIteratorLevel pageIteratorLevel = PageIteratorLevel.RIL_PARA;
                        do
                        {
                            stringBuilder.Append(resultIterator.GetUTF8Text(pageIteratorLevel));
                        } while (resultIterator.Next(pageIteratorLevel));

                        myString = stringBuilder.ToString();

                        myString = Regex.Replace(myString, @"\s+", " ", RegexOptions.Multiline);
                        if (myString == "")
                        {
                            skip = true;
                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("asfas");
                    //}

                    if (!skip)
                    {
                        matrix[i][j] = myString;
                    }

                    j++;
                }

                i++;
            }

            string txtPath = dir + "out.csv";

            using (FileStream fs = File.Create(txtPath))
            {
                for (i = 0; i < matrix.Count; i++)
                {
                    for (j = 0; j < matrix[i].Count; j++)
                    {
                        Byte[] info = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes(matrix[i][j] + "; ");
                        fs.Write(info, 0, info.Length);
                    }
                    Byte[] newLine = System.Text.Encoding.GetEncoding(ci.TextInfo.ANSICodePage).GetBytes(Environment.NewLine);
                    fs.Write(newLine, 0, newLine.Length);
                }
            }

            Process.Start("explorer.exe", "\"" + txtPath);
        }

        private void buttonCapturar_Click(object sender, EventArgs e)
        {
            this.Hide();
            System.Threading.Thread.Sleep(500);

            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            System.Drawing.Graphics gfxScreenshot = System.Drawing.Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            fsc = new FormScreenCapture(bmpScreenshot, gfxScreenshot, this);

            if (!fsc.IsDisposed)
            {
                fsc.Show();
                fsc.UpdatePainting();
            }
        }

        private void buttonBinarizar_Click(object sender, EventArgs e)
        {
            int threshold = Int32.Parse(textBoxToleranciaBinaria.Text);
            Image<Gray, Byte> img = new Image<Gray, byte>((Bitmap)imgTabelaNonBinarized);
            imgTabelaOriginal = img.Convert<Gray, byte>().ThresholdBinary(new Gray(threshold), new Gray(255)).ToBitmap();
            UpdatePainting();
        }
    }
}
