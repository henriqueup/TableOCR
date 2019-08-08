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

namespace TableOCR_0._2
{
    public partial class FormScreenCapture : Form
    {
        public Form1 pai;
        public Bitmap bmpOriginal;
        public Graphics gfxOriginal;
        public Bitmap bmpScreenshot;
        public Graphics gfxScreenshot;
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
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            DoubleBuffered = true;
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
            Bitmap bmpWhite = new Bitmap(bmpOriginal.Width, bmpOriginal.Height, PixelFormat.Format24bppRgb);
            using (Graphics gfxWhite = Graphics.FromImage(bmpWhite))
            {
                gfxWhite.FillRectangle(Brushes.Crimson, 0, 0, bmpWhite.Width, bmpWhite.Height);

                Bitmap bmpBackup = bmpScreenshot;
                Graphics gfxBackup = gfxScreenshot;
                bmpScreenshot = bmpWhite;
                gfxScreenshot = gfxWhite;
                UpdatePainting();

                Wait(100);

                bmpScreenshot = bmpBackup;
                gfxScreenshot = gfxBackup;
                UpdatePainting();
            }
        }

        private void FormScreenshot_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.bmpScreenshot, 0, 0);
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

                Bitmap bmpCapture = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                Graphics gfxCapture = Graphics.FromImage(bmpCapture);
                
                gfxCapture.CopyFromScreen(rect.Left+1, rect.Top+1, 2, 2, new Size(rect.Width, rect.Height));
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
                bmpScreenshot = bmpOriginal;
                gfxScreenshot = gfxOriginal;

                int width = Cursor.Position.X - clicked.X;
                int height = Cursor.Position.Y - clicked.Y;

                if (width < 0)
                    width *= -1;
                if (height < 0)
                    height *= -1;

                rect.Width = width;
                rect.Height = height;

                Refresh();
            }
        }
    }
}
