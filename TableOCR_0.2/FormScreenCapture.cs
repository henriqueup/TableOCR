using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TableOCR_0._2
{
    public partial class FormScreenCapture : Form
    {
        public Form1 pai;
        public Bitmap bmpOriginal;
        public Graphics gfxOriginal;
        public Bitmap bmpScreenshot;
        public Graphics gfxScreenshot;
        private Bitmap canvas;
        private Point clicked;
        private bool mouseIsDown;
        private Rectangle rect;

        public FormScreenCapture(Bitmap bmpScreenshot, Graphics gfxScreenshot, Form1 pai)
        {
            InitializeComponent();
            this.Cursor = Cursors.Cross;
            this.pai = pai;
            this.bmpScreenshot = bmpScreenshot;
            this.gfxScreenshot = gfxScreenshot;
            bmpOriginal = bmpScreenshot;
            gfxOriginal = gfxScreenshot;

            SetOpacity();

            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            DoubleBuffered = true;
        }

        private void SetOpacity()
        {
            canvas = (Bitmap)bmpScreenshot.Clone();
            Bitmap bmpWhite = new Bitmap(canvas.Width, canvas.Height);
            using (Graphics gfx = Graphics.FromImage(canvas))
            using (Graphics gfxWhite = Graphics.FromImage(bmpWhite))
            {
                gfxWhite.Clear(Color.Transparent);

                Region region = new Region(new Rectangle(0, 0, canvas.Width, canvas.Height));
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(rect);
                region.Exclude(path);
                gfxWhite.SetClip(region, CombineMode.Replace);
                gfxWhite.FillRectangle(Brushes.White, 0, 0, canvas.Width, canvas.Height);

                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.58f;

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                gfx.DrawImage(bmpWhite, new Rectangle(0, 0, canvas.Width, canvas.Height), 0, 0, bmpWhite.Width, bmpWhite.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        public void Wait(int milliseconds)
        {
            Timer timer = new Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            //Console.WriteLine("start wait timer");
            timer.Interval = milliseconds;
            timer.Enabled = true;
            timer.Start();
            timer.Tick += (s, e) =>
            {
                timer.Enabled = false;
                timer.Stop();
                //Console.WriteLine("stop wait timer");
            };
            while (timer.Enabled)
            {
                Application.DoEvents();
            }
        }

        public void Blink_Screen()
        {
            Bitmap bmpCrimson = new Bitmap(bmpOriginal.Width, bmpOriginal.Height, PixelFormat.Format24bppRgb);
            using (Graphics gfxCrimson = Graphics.FromImage(bmpCrimson))
            {
                gfxCrimson.FillRectangle(Brushes.Crimson, 0, 0, bmpCrimson.Width, bmpCrimson.Height);

                Bitmap bmpBackup = (Bitmap)bmpScreenshot.Clone();
                Graphics gfxBackup = gfxScreenshot;
                bmpScreenshot = (Bitmap)bmpCrimson.Clone();
                gfxScreenshot = gfxCrimson;
                UpdatePainting();

                Wait(100);

                bmpScreenshot = (Bitmap)bmpBackup.Clone();
                gfxScreenshot = gfxBackup;
                UpdatePainting();
            }
        }

        private void FormScreenshot_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.canvas, 0, 0);
            e.Graphics.DrawRectangle(new Pen(Color.Green, 2), rect);
        }

        public void UpdatePainting()
        {
            //Console.WriteLine("printing");
            this.Invalidate();
            this.Paint += new PaintEventHandler(this.FormScreenshot_Paint);
        }

        private void FormScreenCapture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseIsDown = true;
                clicked = new Point(Cursor.Position.X, Cursor.Position.Y);
                rect = new Rectangle(clicked, new Size(0, 0));
            }
        }

        private void FormScreenCapture_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && rect.Width > 0 && rect.Height > 0)
            {
                mouseIsDown = false;

                Bitmap bmpCapture = new Bitmap(rect.Width-2, rect.Height-2, PixelFormat.Format32bppArgb);
                Graphics gfxCapture = Graphics.FromImage(bmpCapture);
                
                gfxCapture.CopyFromScreen(rect.Left+1, rect.Top+1, 0, 0, new Size(rect.Width-2, rect.Height-2));
                pai.LoadImage(new Image<Gray, Byte>(bmpCapture));
                
                pai.Show();
                pai.Activate();
                pai.WindowState = FormWindowState.Normal;
                
                this.Close();
                this.Dispose();
            }
        }

        private void FormScreenCapture_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                bmpScreenshot = (Bitmap)bmpOriginal.Clone();
                gfxScreenshot = gfxOriginal;

                int width = Cursor.Position.X - clicked.X;
                int height = Cursor.Position.Y - clicked.Y;

                if (width < 0)
                    width *= -1;
                if (height < 0)
                    height *= -1;

                rect.Width = width;
                rect.Height = height;

                SetOpacity();
                Refresh();
            }
        }
    }
}
