namespace Draw_dinamic_line
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Готово";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(204, 337);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 37);
            this.button2.TabIndex = 2;
            this.button2.Text = "Поворот примитива";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 380);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 37);
            this.button3.TabIndex = 3;
            this.button3.Text = "Поворот ребра";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(204, 380);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 37);
            this.button4.TabIndex = 4;
            this.button4.Text = "Пересечение ребер";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(384, 380);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(151, 37);
            this.button5.TabIndex = 5;
            this.button5.Text = "Точка-ребро";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 423);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(170, 37);
            this.button6.TabIndex = 6;
            this.button6.Text = "Точка/многоугольник";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(626, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(108, 22);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "Точка";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(626, 109);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(108, 22);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "Смещение";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(626, 148);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(107, 22);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "Масштаб";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(626, 71);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(108, 22);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "Угол";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(384, 337);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(151, 37);
            this.button8.TabIndex = 12;
            this.button8.Text = "Смещение";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(556, 337);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(153, 37);
            this.button9.TabIndex = 13;
            this.button9.Text = "Масштаб";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 465);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
    }
}

