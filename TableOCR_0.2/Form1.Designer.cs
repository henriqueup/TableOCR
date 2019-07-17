namespace TableOCR_0._2
{
    partial class Form1
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
            this.buttonArquivo = new System.Windows.Forms.Button();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.checkBoxLinha = new System.Windows.Forms.CheckBox();
            this.buttonLinhas = new System.Windows.Forms.Button();
            this.buttonTesseract = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFiltro = new System.Windows.Forms.Button();
            this.buttonCapturar = new System.Windows.Forms.Button();
            this.buttonBinarizar = new System.Windows.Forms.Button();
            this.buttonResetar = new System.Windows.Forms.Button();
            this.buttonDesbinarizar = new System.Windows.Forms.Button();
            this.trackBarTolerancia = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTolerancia)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonArquivo
            // 
            this.buttonArquivo.Location = new System.Drawing.Point(98, 41);
            this.buttonArquivo.Name = "buttonArquivo";
            this.buttonArquivo.Size = new System.Drawing.Size(120, 23);
            this.buttonArquivo.TabIndex = 0;
            this.buttonArquivo.Text = "Carregar Arquivo";
            this.buttonArquivo.UseVisualStyleBackColor = true;
            this.buttonArquivo.Click += new System.EventHandler(this.buttonArquivo_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Location = new System.Drawing.Point(780, 41);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(75, 23);
            this.buttonRedo.TabIndex = 10;
            this.buttonRedo.Text = "Redo";
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            // 
            // buttonUndo
            // 
            this.buttonUndo.Location = new System.Drawing.Point(780, 12);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(75, 23);
            this.buttonUndo.TabIndex = 9;
            this.buttonUndo.Text = "Undo";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(476, 41);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(120, 23);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // checkBoxLinha
            // 
            this.checkBoxLinha.AutoSize = true;
            this.checkBoxLinha.Location = new System.Drawing.Point(602, 15);
            this.checkBoxLinha.Name = "checkBoxLinha";
            this.checkBoxLinha.Size = new System.Drawing.Size(84, 17);
            this.checkBoxLinha.TabIndex = 7;
            this.checkBoxLinha.Text = "Linha Inteira";
            this.checkBoxLinha.UseVisualStyleBackColor = true;
            // 
            // buttonLinhas
            // 
            this.buttonLinhas.Location = new System.Drawing.Point(476, 12);
            this.buttonLinhas.Name = "buttonLinhas";
            this.buttonLinhas.Size = new System.Drawing.Size(120, 23);
            this.buttonLinhas.TabIndex = 6;
            this.buttonLinhas.Text = "Linhas";
            this.buttonLinhas.UseVisualStyleBackColor = true;
            this.buttonLinhas.Click += new System.EventHandler(this.buttonLinhas_Click);
            // 
            // buttonTesseract
            // 
            this.buttonTesseract.Location = new System.Drawing.Point(602, 41);
            this.buttonTesseract.Name = "buttonTesseract";
            this.buttonTesseract.Size = new System.Drawing.Size(120, 23);
            this.buttonTesseract.TabIndex = 11;
            this.buttonTesseract.Text = "Tesseract";
            this.buttonTesseract.UseVisualStyleBackColor = true;
            this.buttonTesseract.Click += new System.EventHandler(this.buttonTesseract_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(350, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tolerância:";
            // 
            // buttonFiltro
            // 
            this.buttonFiltro.Location = new System.Drawing.Point(350, 41);
            this.buttonFiltro.Name = "buttonFiltro";
            this.buttonFiltro.Size = new System.Drawing.Size(120, 23);
            this.buttonFiltro.TabIndex = 14;
            this.buttonFiltro.Text = "Filtro";
            this.buttonFiltro.UseVisualStyleBackColor = true;
            this.buttonFiltro.Click += new System.EventHandler(this.buttonFiltro_Click);
            // 
            // buttonCapturar
            // 
            this.buttonCapturar.Location = new System.Drawing.Point(98, 12);
            this.buttonCapturar.Name = "buttonCapturar";
            this.buttonCapturar.Size = new System.Drawing.Size(120, 23);
            this.buttonCapturar.TabIndex = 16;
            this.buttonCapturar.Text = "Capturar Tela";
            this.buttonCapturar.UseVisualStyleBackColor = true;
            this.buttonCapturar.Click += new System.EventHandler(this.buttonCapturar_Click);
            // 
            // buttonBinarizar
            // 
            this.buttonBinarizar.Location = new System.Drawing.Point(224, 12);
            this.buttonBinarizar.Name = "buttonBinarizar";
            this.buttonBinarizar.Size = new System.Drawing.Size(120, 23);
            this.buttonBinarizar.TabIndex = 19;
            this.buttonBinarizar.Text = "Binarizar";
            this.buttonBinarizar.UseVisualStyleBackColor = true;
            this.buttonBinarizar.Click += new System.EventHandler(this.buttonBinarizar_Click);
            // 
            // buttonResetar
            // 
            this.buttonResetar.Location = new System.Drawing.Point(12, 12);
            this.buttonResetar.Name = "buttonResetar";
            this.buttonResetar.Size = new System.Drawing.Size(75, 52);
            this.buttonResetar.TabIndex = 20;
            this.buttonResetar.Text = "Resetar";
            this.buttonResetar.UseVisualStyleBackColor = true;
            this.buttonResetar.Click += new System.EventHandler(this.buttonResetar_Click);
            // 
            // buttonDesbinarizar
            // 
            this.buttonDesbinarizar.Location = new System.Drawing.Point(224, 41);
            this.buttonDesbinarizar.Name = "buttonDesbinarizar";
            this.buttonDesbinarizar.Size = new System.Drawing.Size(120, 23);
            this.buttonDesbinarizar.TabIndex = 21;
            this.buttonDesbinarizar.Text = "Desbinarizar";
            this.buttonDesbinarizar.UseVisualStyleBackColor = true;
            this.buttonDesbinarizar.Click += new System.EventHandler(this.buttonDesbinarizar_Click);
            // 
            // trackBarTolerancia
            // 
            this.trackBarTolerancia.Location = new System.Drawing.Point(408, 12);
            this.trackBarTolerancia.Name = "trackBarTolerancia";
            this.trackBarTolerancia.Size = new System.Drawing.Size(62, 45);
            this.trackBarTolerancia.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 156);
            this.Controls.Add(this.buttonFiltro);
            this.Controls.Add(this.trackBarTolerancia);
            this.Controls.Add(this.buttonDesbinarizar);
            this.Controls.Add(this.buttonResetar);
            this.Controls.Add(this.buttonBinarizar);
            this.Controls.Add(this.buttonCapturar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonTesseract);
            this.Controls.Add(this.buttonRedo);
            this.Controls.Add(this.buttonUndo);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.checkBoxLinha);
            this.Controls.Add(this.buttonLinhas);
            this.Controls.Add(this.buttonArquivo);
            this.Name = "Form1";
            this.Text = "Tabela OCR";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTolerancia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonArquivo;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.CheckBox checkBoxLinha;
        private System.Windows.Forms.Button buttonLinhas;
        private System.Windows.Forms.Button buttonTesseract;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonFiltro;
        private System.Windows.Forms.Button buttonCapturar;
        private System.Windows.Forms.Button buttonBinarizar;
        private System.Windows.Forms.Button buttonResetar;
        private System.Windows.Forms.Button buttonDesbinarizar;
        private System.Windows.Forms.TrackBar trackBarTolerancia;
    }
}

