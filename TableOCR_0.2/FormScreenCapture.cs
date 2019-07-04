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
            this.pai = pai;
            this.bmpScreenshot = bmpScreenshot;
            this.gfxScreenshot = gfxScreenshot;
            this.bmpOriginal = bmpScreenshot;
            this.gfxOriginal = gfxScreenshot;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
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
