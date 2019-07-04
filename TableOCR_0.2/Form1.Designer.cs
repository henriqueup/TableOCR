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
            this.textBoxTolerancia = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFiltro = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCapturar = new System.Windows.Forms.Button();
            this.textBoxT1 = new System.Windows.Forms.TextBox();
            this.textBoxT2 = new System.Windows.Forms.TextBox();
            this.buttonBinarizar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxToleranciaBinaria = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonArquivo
            // 
            this.buttonArquivo.Location = new System.Drawing.Point(87, 41);
            this.buttonArquivo.Name = "buttonArquivo";
            this.buttonArquivo.Size = new System.Drawing.Size(75, 23);
            this.buttonArquivo.TabIndex = 0;
            this.buttonArquivo.Text = "Arquivo";
            this.buttonArquivo.UseVisualStyleBackColor = true;
            this.buttonArquivo.Click += new System.EventHandler(this.buttonArquivo_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Location = new System.Drawing.Point(580, 41);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(75, 23);
            this.buttonRedo.TabIndex = 10;
            this.buttonRedo.Text = "Redo";
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            // 
            // buttonUndo
            // 
            this.buttonUndo.Location = new System.Drawing.Point(499, 41);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(75, 23);
            this.buttonUndo.TabIndex = 9;
            this.buttonUndo.Text = "Undo";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(418, 41);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // checkBoxLinha
            // 
            this.checkBoxLinha.AutoSize = true;
            this.checkBoxLinha.Location = new System.Drawing.Point(665, 16);
            this.checkBoxLinha.Name = "checkBoxLinha";
            this.checkBoxLinha.Size = new System.Drawing.Size(84, 17);
            this.checkBoxLinha.TabIndex = 7;
            this.checkBoxLinha.Text = "Linha Inteira";
            this.checkBoxLinha.UseVisualStyleBackColor = true;
            // 
            // buttonLinhas
            // 
            this.buttonLinhas.Location = new System.Drawing.Point(580, 12);
            this.buttonLinhas.Name = "buttonLinhas";
            this.buttonLinhas.Size = new System.Drawing.Size(75, 23);
            this.buttonLinhas.TabIndex = 6;
            this.buttonLinhas.Text = "Linhas";
            this.buttonLinhas.UseVisualStyleBackColor = true;
            this.buttonLinhas.Click += new System.EventHandler(this.buttonLinhas_Click);
            // 
            // buttonTesseract
            // 
            this.buttonTesseract.Location = new System.Drawing.Point(12, 41);
            this.buttonTesseract.Name = "buttonTesseract";
            this.buttonTesseract.Size = new System.Drawing.Size(75, 23);
            this.buttonTesseract.TabIndex = 11;
            this.buttonTesseract.Text = "Tesseract";
            this.buttonTesseract.UseVisualStyleBackColor = true;
            this.buttonTesseract.Click += new System.EventHandler(this.buttonTesseract_Click);
            // 
            // textBoxTolerancia
            // 
            this.textBoxTolerancia.Location = new System.Drawing.Point(436, 12);
            this.textBoxTolerancia.Name = "textBoxTolerancia";
            this.textBoxTolerancia.Size = new System.Drawing.Size(57, 20);
            this.textBoxTolerancia.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(370, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tolerância:";
            // 
            // buttonFiltro
            // 
            this.buttonFiltro.Location = new System.Drawing.Point(499, 12);
            this.buttonFiltro.Name = "buttonFiltro";
            this.buttonFiltro.Size = new System.Drawing.Size(75, 23);
            this.buttonFiltro.TabIndex = 14;
            this.buttonFiltro.Text = "Filtro";
            this.buttonFiltro.UseVisualStyleBackColor = true;
            this.buttonFiltro.Click += new System.EventHandler(this.buttonFiltro_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(665, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 15;
            // 
            // buttonCapturar
            // 
            this.buttonCapturar.Location = new System.Drawing.Point(12, 12);
            this.buttonCapturar.Name = "buttonCapturar";
            this.buttonCapturar.Size = new System.Drawing.Size(150, 23);
            this.buttonCapturar.TabIndex = 16;
            this.buttonCapturar.Text = "Capturar Tela";
            this.buttonCapturar.UseVisualStyleBackColor = true;
            this.buttonCapturar.Click += new System.EventHandler(this.buttonCapturar_Click);
            // 
            // textBoxT1
            // 
            this.textBoxT1.Location = new System.Drawing.Point(175, 14);
            this.textBoxT1.Name = "textBoxT1";
            this.textBoxT1.Size = new System.Drawing.Size(35, 20);
            this.textBoxT1.TabIndex = 17;
            // 
            // textBoxT2
            // 
            this.textBoxT2.Location = new System.Drawing.Point(175, 40);
            this.textBoxT2.Name = "textBoxT2";
            this.textBoxT2.Size = new System.Drawing.Size(35, 20);
            this.textBoxT2.TabIndex = 18;
            // 
            // buttonBinarizar
            // 
            this.buttonBinarizar.Location = new System.Drawing.Point(282, 41);
            this.buttonBinarizar.Name = "buttonBinarizar";
            this.buttonBinarizar.Size = new System.Drawing.Size(75, 23);
            this.buttonBinarizar.TabIndex = 19;
            this.buttonBinarizar.Text = "Binarizar";
            this.buttonBinarizar.UseVisualStyleBackColor = true;
            this.buttonBinarizar.Click += new System.EventHandler(this.buttonBinarizar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Tolerância:";
            // 
            // textBoxToleranciaBinaria
            // 
            this.textBoxToleranciaBinaria.Location = new System.Drawing.Point(282, 14);
            this.textBoxToleranciaBinaria.Name = "textBoxToleranciaBinaria";
            this.textBoxToleranciaBinaria.Size = new System.Drawing.Size(57, 20);
            this.textBoxToleranciaBinaria.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 156);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxToleranciaBinaria);
            this.Controls.Add(this.buttonBinarizar);
            this.Controls.Add(this.textBoxT2);
            this.Controls.Add(this.textBoxT1);
            this.Controls.Add(this.buttonCapturar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonFiltro);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTolerancia);
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
        private System.Windows.Forms.TextBox textBoxTolerancia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonFiltro;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonCapturar;
        private System.Windows.Forms.TextBox textBoxT1;
        private System.Windows.Forms.TextBox textBoxT2;
        private System.Windows.Forms.Button buttonBinarizar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxToleranciaBinaria;
    }
}

