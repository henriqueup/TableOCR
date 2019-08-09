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
        //private string path;
        public Image imgTabelaOriginal = null;
        private Image imgTabelaNoLines = null;
        private Image imgTabelaLines = null;
        private System.Drawing.Graphics gfxTabela;
        private Size sizeOriginal;
        static CultureInfo ci = new CultureInfo("pt-BR");
        private int curHist;
        private int curUndoHist;
        int houghTolerance = 0;
        private bool curImgHasLines;
        private bool showOriginalImage;
        private List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>> hist = new List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>>();
        private List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>> undoHist = new List<Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>>();
        private List<Rectangle> boxList;
        private List<List<Rectangle>> cells = new List<List<Rectangle>>();
        private HashSet<Rectangle> removeRectangles = new HashSet<Rectangle>();

        //Form construction
        //Sets initial values for some control variables
        public Form1()
        {
            InitializeComponent();

            //Integers for traversing in the list of history
            curHist = 0;
            curUndoHist = 0;

            //Variable to hold original size of the loaded image
            sizeOriginal = this.Size;

            //Control variables to enable switching between images with or without filters or lines
            curImgHasLines = false;
            showOriginalImage = true;
        }

        //Method called when the form is deactivated
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            //this.Show();
        }

        //Mehod called when the form is activated
        private void Form1_Activate(object sender, EventArgs e)
        {
            //if (!fsc.IsDisposed)
            //{
            //    fsc.WindowState = FormWindowState.Minimized;
            //}
        }

        //Function to add the current state to the history list
        private void AddHist()
        {
            //hist.Add(new Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>((Image)imgTabelaLines.Clone(), new List<Tuple<Point, Point>>(oldLines), new List<Tuple<Point, Point>>(lines)));
            curHist++;
        }

        //Function to add the current state to the undo-history list
        private void AddUndoHist()
        {
            //undoHist.Add(new Tuple<Image, List<Tuple<Point, Point>>, List<Tuple<Point, Point>>>((Image)imgTabelaLines.Clone(), new List<Tuple<Point, Point>>(oldLines), new List<Tuple<Point, Point>>(lines)));
            curUndoHist++;
        }
        
        //Method called whenever the mouse is clicked
        //It checks whether the point clicked has lines or segments, highlighting or de-highlighting them
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //The coordinates of the point clicked on the image are set by subtracting the position
                //of the form on the screen and the space between the border and the image from the position
                //of the cursor when clicked
                //textBox1.Text = (Cursor.Position.X - this.Left - borderLeft) + ", " + (Cursor.Position.Y - this.Top - 80 - borderTop);
                Point clicked = new Point(Cursor.Position.X - this.Left - borderLeft, Cursor.Position.Y - this.Top - 80 - borderTop);

                //A list with the two poits that define each line within a 3 pixel range of the point clicked
                //List<Tuple<Point, Point>> nearLines = FindLinesNear(clicked);

                ////If there are any lines near the point clicked, they are colored accordingly
                //if (nearLines.Count > 0)
                //{
                //    Pen bluePen = new Pen(Color.Blue, 3);
                //    Pen redPen = new Pen(Color.Red, 3);
                //    foreach (Tuple<Point, Point> t in nearLines)
                //    {
                //        if (removeLines.Add(t))
                //        {
                //            //The line was not already in the hash with lines to be removed,
                //            //so it is colored for removal
                //            gfxTabela.DrawLine(bluePen, t.Item1, t.Item2);
                //        }
                //        else
                //        {
                //            //The line was already in the hash, so it is colored back to normal
                //            //and removed from the removal hash
                //            gfxTabela.DrawLine(redPen, t.Item1, t.Item2);
                //            removeLines.Remove(t);
                //        }
                //    }
                //    UpdatePainting();
                //}
            }
        }

        //Method called with Paint Event for drawing images on the form
        //It checks whether it should draw the original image or the ones with or without lines
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (showOriginalImage && imgTabelaOriginal != null)
            {
                e.Graphics.DrawImage(this.imgTabelaOriginal, 0, 80);
            }
            else
            {
                if (curImgHasLines && imgTabelaLines != null)
                    e.Graphics.DrawImage(this.imgTabelaLines, 0, 80);
                else if (imgTabelaNoLines != null)
                    e.Graphics.DrawImage(this.imgTabelaNoLines, 0, 80);
            }
        }

        //Function used to call the Paint event, redrawing the image
        public void UpdatePainting()
        {
            this.Invalidate();
            this.Paint += new PaintEventHandler(this.Form1_Paint);
        }

        //Funtion used to draw a line between two points on the image, given the x and y values of each point
        public void UpdateGfx(int x1, int x2, int y1, int y2)
        {
            Pen redPen = new Pen(Color.Red, 2);
            gfxTabela.DrawLine(redPen, x1, y1, x2, y2);
        }

        //Function that loads the image
        //It recieves an Image<Gray, Byte>, redimensions it and paints it on the form
        public void LoadImage(Image<Gray, Byte> img)
        {
            //int threshold = 50;
            //imgTabelaOriginal = img.Convert<Gray, byte>().ThresholdBinary(new Gray(threshold), new Gray(255)).ToBitmap();
            imgTabelaOriginal = img.ToBitmap();
            int width = imgTabelaOriginal.Size.Width;
            int height = imgTabelaOriginal.Size.Height;

            //Adjusts the height and width to be proportional and having one of them equal the HD dimensions
            if (height > width)
            {
                double ratio = (double)width / (double)height;
                int targetHeight = Screen.PrimaryScreen.Bounds.Height - 100;
                int targetWidth = Convert.ToInt32(ratio * targetHeight);
                imgTabelaOriginal = ResizeImage(imgTabelaOriginal, targetWidth, targetHeight);
            }
            else
            {
                double ratio = (double)height / (double)width;
                int targetWidth = Screen.PrimaryScreen.Bounds.Width - 50;
                int targetHeight = Convert.ToInt32(ratio * targetWidth);
                imgTabelaOriginal = ResizeImage(imgTabelaOriginal, targetWidth, targetHeight);
            }

            if (imgTabelaOriginal.Size.Width + 20 > sizeOriginal.Width)
            {
                this.Width = imgTabelaOriginal.Size.Width + 20;
            }
            else
            {
                this.Width = sizeOriginal.Width;
            }
            if (imgTabelaOriginal.Size.Height + 123 > sizeOriginal.Height)
            {
                this.Height = imgTabelaOriginal.Size.Height + 123;
            }
            else
            {
                this.Height = sizeOriginal.Height;
            }
            UpdatePainting();
        }

        //Method called when the load file "Arquivo" button is clicked
        //It opens the file and loads it on the form
        private void buttonArquivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecione a Imagem";
            ofd.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg";
            //ofd.InitialDirectory = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //path = ofd.FileName;
                LoadImage(new Image<Gray, Byte>(ofd.FileName));
                //LoadImage(Image.FromFile(path));
            }
        }

        //Method called when the button for showing or omiting the lines "Linhas" is clicked
        private void buttonLinhas_Click(object sender, EventArgs e)
        {
            curImgHasLines = !curImgHasLines;
            UpdatePainting();
        }
        
        //Method called when the button for removing the selected lines "Remover" is clicked
        //It removes from the list of lines or segments each one that is in the remove list
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (imgTabelaOriginal == null)
            {
                MessageBox.Show("It is necessary to load an image first.", "Error");
                return;
            }

            
        }

        //Attempt at adding a undo-redo system
        private void buttonUndo_Click(object sender, EventArgs e)
        {
            //if (curHist != 0)
            //{
            //    removeLines.Clear();
            //    AddUndoHist();
            //    imgTabelaLines = (Image)hist[curHist - 1].Item1.Clone();
            //    gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
            //    oldLines = new List<Tuple<Point, Point>>(hist[curHist - 1].Item2);
            //    lines = new List<Tuple<Point, Point>>(hist[curHist - 1].Item3);
            //    UpdatePainting();
            //    hist.RemoveAt(hist.Count - 1);
            //    curHist--;
            //}
        }

        //Attempt at adding a undo-redo system
        private void buttonRedo_Click(object sender, EventArgs e)
        {
            //if (curUndoHist != 0)
            //{
            //    removeLines.Clear();
            //    AddHist();
            //    imgTabelaLines = (Image)undoHist[curUndoHist - 1].Item1.Clone();
            //    gfxTabela = System.Drawing.Graphics.FromImage(imgTabelaLines);
            //    oldLines = new List<Tuple<Point, Point>>(undoHist[curUndoHist - 1].Item2);
            //    lines = new List<Tuple<Point, Point>>(undoHist[curUndoHist - 1].Item3);
            //    UpdatePainting();
            //    undoHist.RemoveAt(undoHist.Count - 1);
            //    curUndoHist--;
            //}
        }

        //Function that converts a System.Drawing.Image to an Emgu.CV.Image<Bgra, byte>
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

        //Function that, given an Image and the values for width and height,
        //return the equivalent image with the input dimensions
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

        //Function used to implement the CvInvoke.FindContours method
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
        
        //Method called when the button for applying the OCR "Tesseract" is clicked
        //It will create the cells structure and go through it, trying to extract the content
        //from the image in the region delimited by the cell and then write it on a CSV file
        //which is openned once the whole process is finished
        private void buttonTesseract_Click(object sender, EventArgs e)
        {
            if (imgTabelaOriginal == null)
            {
                MessageBox.Show("It is necessary to load an image first.", "Error");
                return;
            }

            string dir = string.Empty;
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = @"C:\Users\Public\Desktop";
                fbd.Description = "Select the folder to save the output csv file.";
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    dir = fbd.SelectedPath + "\\";
                }
            }
            string txtPath = dir + "out.csv";

            try
            {
                using (FileStream fs = File.Create(txtPath)) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + "Sugestion: close the previous out.csv file.", "Error");
                return;
            }
            MessageBox.Show("Starting OCR.", "Warning");

            removeRectangles.Clear();
            buttonRemove_Click(this, new EventArgs());

            int i, j;
            bool skip = false;

            string dataPath, language, inputFile;
            //dataPath = @"C:\Tesseract\tesseract-ocr\tessdata";
            dataPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Substring(6) + "\\tessdata";
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
            BuildCells();
            foreach (List<Rectangle> line in cells)
            {
                matrix.Add(new List<string>());

                j = 0;
                foreach (Rectangle cell in line)
                {
                    matrix[i].Add(".");
                    string myString = ".";
                    skip = false;
                    Rectangle roi = cell;
                    if (roi.Height <= 0 || roi.Width <= 0)
                    {
                        skip = true;
                    }

                    if (!skip)
                    {
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
                        File.Delete(dir + "cell_" + i + "," + j + ".jpg");

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

                    if (!skip)
                    {
                        matrix[i][j] = myString;
                    }

                    j++;
                }

                i++;
            }

            using (FileStream fs = File.Create(txtPath))
            {
                //Removes empty lines and columns
                //CleanUpMatrix(ref matrix);

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
                fsc.Blink_Screen();
            }
        }

        //Method called when the reset button "Resetar" is clicked
        //It resets everything, returning the application to its starting state
        private void buttonResetar_Click(object sender, EventArgs e)
        {
            hist.Clear();
            undoHist.Clear();
            cells.Clear();

            imgTabelaOriginal = null;
            imgTabelaNoLines = null;
            imgTabelaLines = null;

            trackBarTolerancia.Value = 0;
            curHist = 0;
            curUndoHist = 0;
            this.Size = sizeOriginal;
            curImgHasLines = false;
            showOriginalImage = true;

            UpdatePainting();
        }

        private void trackBarTolerancia_ValueChanged(object sender, EventArgs e)
        {
            houghTolerance = trackBarTolerancia.Value * 100;
            numericUpDownFineTolerance.Text = houghTolerance.ToString();
        }

        private void numericUpDownFineTolerance_ValueChanged(object sender, EventArgs e)
        {
            houghTolerance = Int32.Parse(numericUpDownFineTolerance.Text);
        }

        private void BuildCells()
        {
            HashSet<int> yValues = new HashSet<int>();

            boxList = boxList.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            foreach (Rectangle rect in boxList)
            {
                if (!yValues.Contains(rect.Y))
                {
                    cells.Add(new List<Rectangle>());
                }

                cells.Last().Add(rect);
                yValues.Add(rect.Y);
            }
        }

        private void buttonFiltro_Click(object sender, EventArgs e)
        {
            //Load the image from file and resize it for display
            Image<Bgr, Byte> img = new Image<Bgr, byte>((Bitmap)imgTabelaOriginal);

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            //Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();
            

            #region Canny and edge detection
            double cannyThreshold = 130;
            double cannyThresholdLinking = 100;
            UMat cannyEdges = new UMat();
            //CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);
            //CvInvoke.Threshold(uimage, cannyEdges, 100, 255, ThresholdType.Otsu);
            CvInvoke.AdaptiveThreshold(uimage, cannyEdges, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 19, 3);

            showOriginalImage = false;
            imgTabelaNoLines = cannyEdges.ToImage<Bgr, byte>().ToBitmap();
            UpdatePainting();
            #endregion
            
            boxList = new List<Rectangle>(); //a box is a rotated rectangle

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                double maxArea = (imgTabelaOriginal.Size.Width - 10) * (imgTabelaOriginal.Size.Height - 10);
                for (int i = 0; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contour);
                        double area = rect.Width * rect.Height;
                        if (area > houghTolerance*2 && area < maxArea)
                            boxList.Add(rect);
                    }
                }
            }

            //originalImageBox.Image = img;
            //this.Text = msgBuilder.ToString();
            
            #region draw lines and rectangles
            Image<Bgr, Byte> linesRectangleImage = img;
            foreach (Rectangle box in boxList)
                linesRectangleImage.Draw(box, new Bgr(Color.Red), 2);
            imgTabelaLines = linesRectangleImage.ToBitmap();
            UpdatePainting();
            #endregion
        }
    }
}
