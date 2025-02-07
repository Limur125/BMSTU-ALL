namespace WinFormsApp1
{
    partial class Form2
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
            label1 = new Label();
            NameL = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            ClassL = new Label();
            label10 = new Label();
            label11 = new Label();
            VaccineL = new Label();
            AgeL = new Label();
            MaxAgeL = new Label();
            ActiveL = new Label();
            ColorL = new Label();
            TypeL = new Label();
            SpecieL = new Label();
            BreedL = new Label();
            ConditionsL = new TextBox();
            label2 = new Label();
            SizeL = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 14);
            label1.Name = "label1";
            label1.Size = new Size(39, 20);
            label1.TabIndex = 0;
            label1.Text = "Имя";
            // 
            // NameL
            // 
            NameL.AutoSize = true;
            NameL.Location = new Point(304, 14);
            NameL.Name = "NameL";
            NameL.Size = new Size(77, 20);
            NameL.TabIndex = 1;
            NameL.Text = "Название";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 34);
            label3.Name = "label3";
            label3.Size = new Size(94, 20);
            label3.TabIndex = 2;
            label3.Text = "Вакцинация";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 54);
            label4.Name = "label4";
            label4.Size = new Size(64, 20);
            label4.TabIndex = 3;
            label4.Text = "Возраст";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 74);
            label5.Name = "label5";
            label5.Size = new Size(262, 20);
            label5.TabIndex = 4;
            label5.Text = "Средняя продолжительность жизни";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 94);
            label6.Name = "label6";
            label6.Size = new Size(75, 20);
            label6.TabIndex = 5;
            label6.Text = "Активное";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 114);
            label7.Name = "label7";
            label7.Size = new Size(42, 20);
            label7.TabIndex = 6;
            label7.Text = "Цвет";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(14, 154);
            label8.Name = "label8";
            label8.Size = new Size(157, 20);
            label8.TabIndex = 7;
            label8.Text = "Условия содержания";
            // 
            // ClassL
            // 
            ClassL.AutoSize = true;
            ClassL.Location = new Point(14, 379);
            ClassL.Name = "ClassL";
            ClassL.Size = new Size(48, 20);
            ClassL.TabIndex = 8;
            ClassL.Text = "Класс";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(14, 399);
            label10.Name = "label10";
            label10.Size = new Size(35, 20);
            label10.TabIndex = 9;
            label10.Text = "Вид";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(14, 419);
            label11.Name = "label11";
            label11.Size = new Size(63, 20);
            label11.TabIndex = 10;
            label11.Text = "Порода";
            // 
            // VaccineL
            // 
            VaccineL.AutoSize = true;
            VaccineL.Location = new Point(304, 34);
            VaccineL.Name = "VaccineL";
            VaccineL.Size = new Size(48, 20);
            VaccineL.TabIndex = 11;
            VaccineL.Text = "Класс";
            // 
            // AgeL
            // 
            AgeL.AutoSize = true;
            AgeL.Location = new Point(304, 54);
            AgeL.Name = "AgeL";
            AgeL.Size = new Size(48, 20);
            AgeL.TabIndex = 12;
            AgeL.Text = "Класс";
            AgeL.Click += label13_Click;
            // 
            // MaxAgeL
            // 
            MaxAgeL.AutoSize = true;
            MaxAgeL.Location = new Point(304, 74);
            MaxAgeL.Name = "MaxAgeL";
            MaxAgeL.Size = new Size(48, 20);
            MaxAgeL.TabIndex = 13;
            MaxAgeL.Text = "Класс";
            // 
            // ActiveL
            // 
            ActiveL.AutoSize = true;
            ActiveL.Location = new Point(304, 94);
            ActiveL.Name = "ActiveL";
            ActiveL.Size = new Size(48, 20);
            ActiveL.TabIndex = 14;
            ActiveL.Text = "Класс";
            // 
            // ColorL
            // 
            ColorL.AutoSize = true;
            ColorL.Location = new Point(304, 114);
            ColorL.Name = "ColorL";
            ColorL.Size = new Size(48, 20);
            ColorL.TabIndex = 15;
            ColorL.Text = "Класс";
            // 
            // TypeL
            // 
            TypeL.AutoSize = true;
            TypeL.Location = new Point(304, 379);
            TypeL.Name = "TypeL";
            TypeL.Size = new Size(48, 20);
            TypeL.TabIndex = 17;
            TypeL.Text = "Класс";
            TypeL.Click += TypeL_Click;
            // 
            // SpecieL
            // 
            SpecieL.AutoSize = true;
            SpecieL.Location = new Point(304, 399);
            SpecieL.Name = "SpecieL";
            SpecieL.Size = new Size(48, 20);
            SpecieL.TabIndex = 18;
            SpecieL.Text = "Класс";
            SpecieL.Click += label19_Click;
            // 
            // BreedL
            // 
            BreedL.AutoSize = true;
            BreedL.Location = new Point(304, 419);
            BreedL.Name = "BreedL";
            BreedL.Size = new Size(48, 20);
            BreedL.TabIndex = 19;
            BreedL.Text = "Класс";
            // 
            // ConditionsL
            // 
            ConditionsL.Location = new Point(304, 162);
            ConditionsL.Multiline = true;
            ConditionsL.Name = "ConditionsL";
            ConditionsL.ReadOnly = true;
            ConditionsL.ScrollBars = ScrollBars.Vertical;
            ConditionsL.Size = new Size(269, 214);
            ConditionsL.TabIndex = 21;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 134);
            label2.Name = "label2";
            label2.Size = new Size(157, 20);
            label2.TabIndex = 22;
            label2.Text = "Условия содержания";
            label2.Click += label2_Click;
            // 
            // SizeL
            // 
            SizeL.AutoSize = true;
            SizeL.Location = new Point(304, 139);
            SizeL.Name = "SizeL";
            SizeL.Size = new Size(157, 20);
            SizeL.TabIndex = 23;
            SizeL.Text = "Условия содержания";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(SizeL);
            Controls.Add(label2);
            Controls.Add(ConditionsL);
            Controls.Add(BreedL);
            Controls.Add(SpecieL);
            Controls.Add(TypeL);
            Controls.Add(ColorL);
            Controls.Add(ActiveL);
            Controls.Add(MaxAgeL);
            Controls.Add(AgeL);
            Controls.Add(VaccineL);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(ClassL);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(NameL);
            Controls.Add(label1);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label NameL;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label ClassL;
        private Label label10;
        private Label label11;
        private Label VaccineL;
        private Label AgeL;
        private Label MaxAgeL;
        private Label ActiveL;
        private Label ColorL;
        private Label TypeL;
        private Label SpecieL;
        private Label BreedL;
        private TextBox ConditionsL;
        private Label label2;
        private Label SizeL;
    }
}