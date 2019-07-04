namespace TableOCR_0._2
{
    partial class FormScreenCapture
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "FormScreenCapture";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormScreenCapture_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormScreenCapture_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormScreenCapture_MouseMove);
        }

        #endregion
    }
}