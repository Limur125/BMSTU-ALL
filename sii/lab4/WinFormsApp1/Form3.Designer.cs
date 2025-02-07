namespace WinFormsApp1
{
    partial class Form3
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
            dataGridView1 = new DataGridView();
            NameDGV = new DataGridViewTextBoxColumn();
            Like = new DataGridViewTextBoxColumn();
            dataGridView2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label1 = new Label();
            panel2 = new Panel();
            radioButton4 = new RadioButton();
            radioButton5 = new RadioButton();
            radioButton6 = new RadioButton();
            label2 = new Label();
            panel3 = new Panel();
            label5 = new Label();
            numericUpDown2 = new NumericUpDown();
            label4 = new Label();
            label3 = new Label();
            numericUpDown1 = new NumericUpDown();
            panel4 = new Panel();
            label6 = new Label();
            numericUpDown3 = new NumericUpDown();
            label7 = new Label();
            label8 = new Label();
            numericUpDown4 = new NumericUpDown();
            panel5 = new Panel();
            label9 = new Label();
            numericUpDown5 = new NumericUpDown();
            label10 = new Label();
            label11 = new Label();
            numericUpDown6 = new NumericUpDown();
            dataGridView3 = new DataGridView();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridView4 = new DataGridView();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridView5 = new DataGridView();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView5).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { NameDGV, Like });
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(249, 327);
            dataGridView1.TabIndex = 4;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // NameDGV
            // 
            NameDGV.HeaderText = "Цвет";
            NameDGV.MinimumWidth = 6;
            NameDGV.Name = "NameDGV";
            NameDGV.ReadOnly = true;
            NameDGV.Width = 125;
            // 
            // Like
            // 
            Like.HeaderText = "Фильтр (+/0/-)";
            Like.MinimumWidth = 6;
            Like.Name = "Like";
            Like.Width = 70;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dataGridView2.Location = new Point(267, 12);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(249, 327);
            dataGridView2.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Условия содержания";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Фильтр (+/0/-)";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 70;
            // 
            // panel1
            // 
            panel1.Controls.Add(radioButton3);
            panel1.Controls.Add(radioButton2);
            panel1.Controls.Add(radioButton1);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(12, 345);
            panel1.Name = "panel1";
            panel1.Size = new Size(249, 112);
            panel1.TabIndex = 6;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(3, 83);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(111, 24);
            radioButton3.TabIndex = 3;
            radioButton3.TabStop = true;
            radioButton3.Text = "Спокойный";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(3, 53);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(99, 24);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "Активный";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(3, 23);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(79, 24);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "Любой";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 20);
            label1.TabIndex = 0;
            label1.Text = "Характер";
            // 
            // panel2
            // 
            panel2.Controls.Add(radioButton4);
            panel2.Controls.Add(radioButton5);
            panel2.Controls.Add(radioButton6);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(12, 463);
            panel2.Name = "panel2";
            panel2.Size = new Size(249, 112);
            panel2.TabIndex = 7;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(3, 83);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(55, 24);
            radioButton4.TabIndex = 3;
            radioButton4.TabStop = true;
            radioButton4.Text = "Нет";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Location = new Point(3, 53);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(48, 24);
            radioButton5.TabIndex = 2;
            radioButton5.TabStop = true;
            radioButton5.Text = "Да";
            radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(3, 23);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(79, 24);
            radioButton6.TabIndex = 1;
            radioButton6.TabStop = true;
            radioButton6.Text = "Любой";
            radioButton6.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 0;
            label2.Text = "Вакцинация";
            // 
            // panel3
            // 
            panel3.Controls.Add(label5);
            panel3.Controls.Add(numericUpDown2);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(numericUpDown1);
            panel3.Location = new Point(267, 345);
            panel3.Name = "panel3";
            panel3.Size = new Size(249, 112);
            panel3.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(112, 61);
            label5.Name = "label5";
            label5.Size = new Size(44, 20);
            label5.TabIndex = 4;
            label5.Text = "Макс";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(3, 59);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(103, 27);
            numericUpDown2.TabIndex = 3;
            numericUpDown2.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(112, 28);
            label4.Name = "label4";
            label4.Size = new Size(40, 20);
            label4.TabIndex = 2;
            label4.Text = "Мин";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Size = new Size(64, 20);
            label3.TabIndex = 1;
            label3.Text = "Возраст";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(3, 26);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(103, 27);
            numericUpDown1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(label6);
            panel4.Controls.Add(numericUpDown3);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label8);
            panel4.Controls.Add(numericUpDown4);
            panel4.Location = new Point(522, 345);
            panel4.Name = "panel4";
            panel4.Size = new Size(268, 91);
            panel4.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(112, 61);
            label6.Name = "label6";
            label6.Size = new Size(44, 20);
            label6.TabIndex = 4;
            label6.Text = "Макс";
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(3, 59);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(103, 27);
            numericUpDown3.TabIndex = 3;
            numericUpDown3.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(112, 28);
            label7.Name = "label7";
            label7.Size = new Size(40, 20);
            label7.TabIndex = 2;
            label7.Text = "Мин";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 3);
            label8.Name = "label8";
            label8.Size = new Size(262, 20);
            label8.TabIndex = 1;
            label8.Text = "Средняя продолжительность жизни";
            // 
            // numericUpDown4
            // 
            numericUpDown4.Location = new Point(3, 26);
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(103, 27);
            numericUpDown4.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.Controls.Add(label9);
            panel5.Controls.Add(numericUpDown5);
            panel5.Controls.Add(label10);
            panel5.Controls.Add(label11);
            panel5.Controls.Add(numericUpDown6);
            panel5.Location = new Point(267, 463);
            panel5.Name = "panel5";
            panel5.Size = new Size(249, 112);
            panel5.TabIndex = 10;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(112, 61);
            label9.Name = "label9";
            label9.Size = new Size(69, 20);
            label9.TabIndex = 4;
            label9.Text = "см, Макс";
            // 
            // numericUpDown5
            // 
            numericUpDown5.Location = new Point(3, 59);
            numericUpDown5.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numericUpDown5.Name = "numericUpDown5";
            numericUpDown5.Size = new Size(103, 27);
            numericUpDown5.TabIndex = 3;
            numericUpDown5.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(112, 28);
            label10.Name = "label10";
            label10.Size = new Size(65, 20);
            label10.TabIndex = 2;
            label10.Text = "см, Мин";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(3, 3);
            label11.Name = "label11";
            label11.Size = new Size(60, 20);
            label11.TabIndex = 1;
            label11.Text = "Размер";
            // 
            // numericUpDown6
            // 
            numericUpDown6.Location = new Point(3, 26);
            numericUpDown6.Name = "numericUpDown6";
            numericUpDown6.Size = new Size(103, 27);
            numericUpDown6.TabIndex = 0;
            // 
            // dataGridView3
            // 
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.AllowUserToDeleteRows = false;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dataGridView3.Location = new Point(522, 12);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(268, 327);
            dataGridView3.TabIndex = 11;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Вид";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Фильтр (+/0/-)";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 90;
            // 
            // dataGridView4
            // 
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.AllowUserToDeleteRows = false;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6 });
            dataGridView4.Location = new Point(796, 12);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(268, 327);
            dataGridView4.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Порода";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Фильтр (+/0/-)";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 90;
            // 
            // dataGridView5
            // 
            dataGridView5.AllowUserToAddRows = false;
            dataGridView5.AllowUserToDeleteRows = false;
            dataGridView5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView5.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8 });
            dataGridView5.Location = new Point(796, 345);
            dataGridView5.Name = "dataGridView5";
            dataGridView5.RowHeadersWidth = 51;
            dataGridView5.Size = new Size(268, 232);
            dataGridView5.TabIndex = 13;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Тип";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Фильтр (+/0/-)";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.Width = 90;
            // 
            // button1
            // 
            button1.Location = new Point(568, 478);
            button1.Name = "button1";
            button1.Size = new Size(181, 71);
            button1.TabIndex = 14;
            button1.Text = "Применить фильтр";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1078, 589);
            Controls.Add(button1);
            Controls.Add(dataGridView5);
            Controls.Add(dataGridView4);
            Controls.Add(dataGridView3);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(dataGridView2);
            Controls.Add(dataGridView1);
            Name = "Form3";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView5).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn NameDGV;
        private DataGridViewTextBoxColumn Like;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private Panel panel1;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label1;
        private Panel panel2;
        private RadioButton radioButton4;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private Label label2;
        private Panel panel3;
        private Label label5;
        private NumericUpDown numericUpDown2;
        private Label label4;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private Panel panel4;
        private Label label6;
        private NumericUpDown numericUpDown3;
        private Label label7;
        private Label label8;
        private NumericUpDown numericUpDown4;
        private Panel panel5;
        private Label label9;
        private NumericUpDown numericUpDown5;
        private Label label10;
        private Label label11;
        private NumericUpDown numericUpDown6;
        private DataGridView dataGridView3;
        private DataGridView dataGridView4;
        private DataGridView dataGridView5;
        private Button button1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    }
}