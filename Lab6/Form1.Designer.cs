namespace Lab6
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxTrans = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxAngle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPoint1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPoint2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOkta = new System.Windows.Forms.Button();
            this.buttonHexa = new System.Windows.Forms.Button();
            this.buttonTetra = new System.Windows.Forms.Button();
            this.buttonPlot = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxProjPlane = new System.Windows.Forms.ComboBox();
            this.radioButtonOrtoProj = new System.Windows.Forms.RadioButton();
            this.radioButtonIsoProj = new System.Windows.Forms.RadioButton();
            this.radioButtonCenterProj = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonAxisRot = new System.Windows.Forms.RadioButton();
            this.radioButtonReflect = new System.Windows.Forms.RadioButton();
            this.radioButtonScale = new System.Windows.Forms.RadioButton();
            this.radioButtonRot = new System.Windows.Forms.RadioButton();
            this.radioButtonTrans = new System.Windows.Forms.RadioButton();
            this.comboBoxPlane = new System.Windows.Forms.ComboBox();
            this.comboBoxAxis = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Icosahedron = new System.Windows.Forms.Button();
            this.Dodecahedron = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 340);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxTrans
            // 
            this.textBoxTrans.BackColor = System.Drawing.Color.LimeGreen;
            this.textBoxTrans.Location = new System.Drawing.Point(570, 12);
            this.textBoxTrans.Name = "textBoxTrans";
            this.textBoxTrans.Size = new System.Drawing.Size(83, 20);
            this.textBoxTrans.TabIndex = 1;
            this.textBoxTrans.Text = "0, 0, 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(503, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Смещение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(511, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Масштаб";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(570, 64);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(83, 20);
            this.textBoxScale.TabIndex = 4;
            this.textBoxScale.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(532, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Угол";
            // 
            // textBoxAngle
            // 
            this.textBoxAngle.Location = new System.Drawing.Point(570, 38);
            this.textBoxAngle.Name = "textBoxAngle";
            this.textBoxAngle.Size = new System.Drawing.Size(83, 20);
            this.textBoxAngle.TabIndex = 6;
            this.textBoxAngle.Text = "0";
            this.textBoxAngle.TextChanged += new System.EventHandler(this.textBoxAngle_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(537, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ось";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(502, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Плоскость";
            // 
            // textBoxPoint1
            // 
            this.textBoxPoint1.Location = new System.Drawing.Point(570, 117);
            this.textBoxPoint1.Name = "textBoxPoint1";
            this.textBoxPoint1.Size = new System.Drawing.Size(83, 20);
            this.textBoxPoint1.TabIndex = 14;
            this.textBoxPoint1.Text = "0, 0, 0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(518, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Точка 1";
            // 
            // textBoxPoint2
            // 
            this.textBoxPoint2.Location = new System.Drawing.Point(570, 143);
            this.textBoxPoint2.Name = "textBoxPoint2";
            this.textBoxPoint2.Size = new System.Drawing.Size(83, 20);
            this.textBoxPoint2.TabIndex = 12;
            this.textBoxPoint2.Text = "1, 0, 0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(518, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Точка 2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Icosahedron);
            this.groupBox1.Controls.Add(this.Dodecahedron);
            this.groupBox1.Controls.Add(this.buttonOkta);
            this.groupBox1.Controls.Add(this.buttonHexa);
            this.groupBox1.Controls.Add(this.buttonTetra);
            this.groupBox1.Location = new System.Drawing.Point(504, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 104);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Построить";
            // 
            // buttonOkta
            // 
            this.buttonOkta.Location = new System.Drawing.Point(10, 77);
            this.buttonOkta.Name = "buttonOkta";
            this.buttonOkta.Size = new System.Drawing.Size(75, 23);
            this.buttonOkta.TabIndex = 2;
            this.buttonOkta.Text = "Октаэдр";
            this.buttonOkta.UseVisualStyleBackColor = true;
            this.buttonOkta.Click += new System.EventHandler(this.buttonOkta_Click);
            // 
            // buttonHexa
            // 
            this.buttonHexa.Location = new System.Drawing.Point(10, 48);
            this.buttonHexa.Name = "buttonHexa";
            this.buttonHexa.Size = new System.Drawing.Size(75, 23);
            this.buttonHexa.TabIndex = 1;
            this.buttonHexa.Text = "Гексаэдр";
            this.buttonHexa.UseVisualStyleBackColor = true;
            this.buttonHexa.Click += new System.EventHandler(this.buttonHexa_Click);
            // 
            // buttonTetra
            // 
            this.buttonTetra.Location = new System.Drawing.Point(10, 19);
            this.buttonTetra.Name = "buttonTetra";
            this.buttonTetra.Size = new System.Drawing.Size(75, 23);
            this.buttonTetra.TabIndex = 0;
            this.buttonTetra.Text = "Тетраэдр";
            this.buttonTetra.UseVisualStyleBackColor = true;
            this.buttonTetra.Click += new System.EventHandler(this.buttonTetra_Click);
            // 
            // buttonPlot
            // 
            this.buttonPlot.BackColor = System.Drawing.Color.Lime;
            this.buttonPlot.Location = new System.Drawing.Point(558, 329);
            this.buttonPlot.Name = "buttonPlot";
            this.buttonPlot.Size = new System.Drawing.Size(75, 23);
            this.buttonPlot.TabIndex = 28;
            this.buttonPlot.Text = "Построить";
            this.buttonPlot.UseVisualStyleBackColor = false;
            this.buttonPlot.Click += new System.EventHandler(this.buttonPlot_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxProjPlane);
            this.groupBox2.Controls.Add(this.radioButtonOrtoProj);
            this.groupBox2.Controls.Add(this.radioButtonIsoProj);
            this.groupBox2.Controls.Add(this.radioButtonCenterProj);
            this.groupBox2.Location = new System.Drawing.Point(13, 358);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 99);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Проекции";
            // 
            // comboBoxProjPlane
            // 
            this.comboBoxProjPlane.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxProjPlane.Enabled = false;
            this.comboBoxProjPlane.FormattingEnabled = true;
            this.comboBoxProjPlane.Items.AddRange(new object[] {
            "Oxy",
            "Oxz",
            "Oyz"});
            this.comboBoxProjPlane.Location = new System.Drawing.Point(128, 64);
            this.comboBoxProjPlane.Name = "comboBoxProjPlane";
            this.comboBoxProjPlane.Size = new System.Drawing.Size(52, 21);
            this.comboBoxProjPlane.TabIndex = 3;
            this.comboBoxProjPlane.Text = "Oxy";
            this.comboBoxProjPlane.SelectionChangeCommitted += new System.EventHandler(this.comboBoxProjPlane_SelectionChangeCommitted);
            // 
            // radioButtonOrtoProj
            // 
            this.radioButtonOrtoProj.AutoSize = true;
            this.radioButtonOrtoProj.Location = new System.Drawing.Point(6, 65);
            this.radioButtonOrtoProj.Name = "radioButtonOrtoProj";
            this.radioButtonOrtoProj.Size = new System.Drawing.Size(116, 17);
            this.radioButtonOrtoProj.TabIndex = 2;
            this.radioButtonOrtoProj.Text = "Ортографическая";
            this.radioButtonOrtoProj.UseVisualStyleBackColor = true;
            this.radioButtonOrtoProj.CheckedChanged += new System.EventHandler(this.radioButtonOrtoProj_CheckedChanged);
            // 
            // radioButtonIsoProj
            // 
            this.radioButtonIsoProj.AutoSize = true;
            this.radioButtonIsoProj.Checked = true;
            this.radioButtonIsoProj.Location = new System.Drawing.Point(6, 42);
            this.radioButtonIsoProj.Name = "radioButtonIsoProj";
            this.radioButtonIsoProj.Size = new System.Drawing.Size(111, 17);
            this.radioButtonIsoProj.TabIndex = 1;
            this.radioButtonIsoProj.TabStop = true;
            this.radioButtonIsoProj.Text = "Изометрическая";
            this.radioButtonIsoProj.UseVisualStyleBackColor = true;
            this.radioButtonIsoProj.CheckedChanged += new System.EventHandler(this.radioButtonIsoProj_CheckedChanged);
            // 
            // radioButtonCenterProj
            // 
            this.radioButtonCenterProj.AutoSize = true;
            this.radioButtonCenterProj.Location = new System.Drawing.Point(6, 19);
            this.radioButtonCenterProj.Name = "radioButtonCenterProj";
            this.radioButtonCenterProj.Size = new System.Drawing.Size(104, 17);
            this.radioButtonCenterProj.TabIndex = 0;
            this.radioButtonCenterProj.Text = "Перспективная";
            this.radioButtonCenterProj.UseVisualStyleBackColor = true;
            this.radioButtonCenterProj.CheckedChanged += new System.EventHandler(this.radioButtonCenterProj_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonAxisRot);
            this.groupBox3.Controls.Add(this.radioButtonReflect);
            this.groupBox3.Controls.Add(this.radioButtonScale);
            this.groupBox3.Controls.Add(this.radioButtonRot);
            this.groupBox3.Controls.Add(this.radioButtonTrans);
            this.groupBox3.Location = new System.Drawing.Point(211, 358);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 99);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Преобразования";
            // 
            // radioButtonAxisRot
            // 
            this.radioButtonAxisRot.AutoSize = true;
            this.radioButtonAxisRot.Location = new System.Drawing.Point(111, 42);
            this.radioButtonAxisRot.Name = "radioButtonAxisRot";
            this.radioButtonAxisRot.Size = new System.Drawing.Size(104, 17);
            this.radioButtonAxisRot.TabIndex = 5;
            this.radioButtonAxisRot.Text = "Вращение (ось)";
            this.radioButtonAxisRot.UseVisualStyleBackColor = true;
            this.radioButtonAxisRot.CheckedChanged += new System.EventHandler(this.radioButtonAxisRot_CheckedChanged);
            // 
            // radioButtonReflect
            // 
            this.radioButtonReflect.AutoSize = true;
            this.radioButtonReflect.Location = new System.Drawing.Point(111, 19);
            this.radioButtonReflect.Name = "radioButtonReflect";
            this.radioButtonReflect.Size = new System.Drawing.Size(82, 17);
            this.radioButtonReflect.TabIndex = 3;
            this.radioButtonReflect.Text = "Отражение";
            this.radioButtonReflect.UseVisualStyleBackColor = true;
            this.radioButtonReflect.CheckedChanged += new System.EventHandler(this.radioButtonReflect_CheckedChanged);
            // 
            // radioButtonScale
            // 
            this.radioButtonScale.AutoSize = true;
            this.radioButtonScale.Location = new System.Drawing.Point(6, 65);
            this.radioButtonScale.Name = "radioButtonScale";
            this.radioButtonScale.Size = new System.Drawing.Size(71, 17);
            this.radioButtonScale.TabIndex = 2;
            this.radioButtonScale.Text = "Масштаб";
            this.radioButtonScale.UseVisualStyleBackColor = true;
            this.radioButtonScale.CheckedChanged += new System.EventHandler(this.radioButtonScale_CheckedChanged);
            // 
            // radioButtonRot
            // 
            this.radioButtonRot.AutoSize = true;
            this.radioButtonRot.Location = new System.Drawing.Point(6, 42);
            this.radioButtonRot.Name = "radioButtonRot";
            this.radioButtonRot.Size = new System.Drawing.Size(68, 17);
            this.radioButtonRot.TabIndex = 1;
            this.radioButtonRot.Text = "Поворот";
            this.radioButtonRot.UseVisualStyleBackColor = true;
            this.radioButtonRot.CheckedChanged += new System.EventHandler(this.radioButtonRot_CheckedChanged);
            // 
            // radioButtonTrans
            // 
            this.radioButtonTrans.AutoSize = true;
            this.radioButtonTrans.Checked = true;
            this.radioButtonTrans.Location = new System.Drawing.Point(6, 19);
            this.radioButtonTrans.Name = "radioButtonTrans";
            this.radioButtonTrans.Size = new System.Drawing.Size(79, 17);
            this.radioButtonTrans.TabIndex = 0;
            this.radioButtonTrans.TabStop = true;
            this.radioButtonTrans.Text = "Смещение";
            this.radioButtonTrans.UseVisualStyleBackColor = true;
            this.radioButtonTrans.CheckedChanged += new System.EventHandler(this.radioButtonTrans_CheckedChanged);
            // 
            // comboBoxPlane
            // 
            this.comboBoxPlane.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxPlane.FormattingEnabled = true;
            this.comboBoxPlane.Items.AddRange(new object[] {
            "Oxy",
            "Oxz",
            "Oyz"});
            this.comboBoxPlane.Location = new System.Drawing.Point(571, 169);
            this.comboBoxPlane.Name = "comboBoxPlane";
            this.comboBoxPlane.Size = new System.Drawing.Size(83, 21);
            this.comboBoxPlane.TabIndex = 4;
            this.comboBoxPlane.Text = "Oxy";
            // 
            // comboBoxAxis
            // 
            this.comboBoxAxis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxAxis.FormattingEnabled = true;
            this.comboBoxAxis.Items.AddRange(new object[] {
            "Ox",
            "Oy",
            "Oz"});
            this.comboBoxAxis.Location = new System.Drawing.Point(570, 90);
            this.comboBoxAxis.Name = "comboBoxAxis";
            this.comboBoxAxis.Size = new System.Drawing.Size(83, 21);
            this.comboBoxAxis.TabIndex = 32;
            this.comboBoxAxis.Text = "Ox";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(547, 364);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(93, 93);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // Icosahedron
            // 
            this.Icosahedron.Location = new System.Drawing.Point(91, 19);
            this.Icosahedron.Name = "Icosahedron";
            this.Icosahedron.Size = new System.Drawing.Size(75, 23);
            this.Icosahedron.TabIndex = 34;
            this.Icosahedron.Text = "Икосаэдр";
            this.Icosahedron.UseVisualStyleBackColor = true;
            this.Icosahedron.Click += new System.EventHandler(this.Icosahedron_Click);
            // 
            // Dodecahedron
            // 
            this.Dodecahedron.Location = new System.Drawing.Point(91, 48);
            this.Dodecahedron.Name = "Dodecahedron";
            this.Dodecahedron.Size = new System.Drawing.Size(75, 23);
            this.Dodecahedron.TabIndex = 35;
            this.Dodecahedron.Text = "Додекаэдр";
            this.Dodecahedron.UseVisualStyleBackColor = true;
            this.Dodecahedron.Click += new System.EventHandler(this.Dodecahedron_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 462);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.comboBoxAxis);
            this.Controls.Add(this.comboBoxPlane);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonPlot);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPoint1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPoint2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxAngle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTrans);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxTrans;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAngle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPoint1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPoint2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOkta;
        private System.Windows.Forms.Button buttonHexa;
        private System.Windows.Forms.Button buttonTetra;
        private System.Windows.Forms.Button buttonPlot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonOrtoProj;
        private System.Windows.Forms.RadioButton radioButtonIsoProj;
        private System.Windows.Forms.RadioButton radioButtonCenterProj;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonAxisRot;
        private System.Windows.Forms.RadioButton radioButtonReflect;
        private System.Windows.Forms.RadioButton radioButtonScale;
        private System.Windows.Forms.RadioButton radioButtonRot;
        private System.Windows.Forms.RadioButton radioButtonTrans;
        private System.Windows.Forms.ComboBox comboBoxProjPlane;
        private System.Windows.Forms.ComboBox comboBoxPlane;
        private System.Windows.Forms.ComboBox comboBoxAxis;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button Icosahedron;
        private System.Windows.Forms.Button Dodecahedron;
    }
}

