namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InputLabel = new Label();
            dataGridView1 = new DataGridView();
            NameDGV = new DataGridViewTextBoxColumn();
            Like = new DataGridViewTextBoxColumn();
            ResLabel = new Label();
            listBox1 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // InputLabel
            // 
            InputLabel.AutoSize = true;
            InputLabel.Location = new Point(12, 9);
            InputLabel.Name = "InputLabel";
            InputLabel.Size = new Size(136, 20);
            InputLabel.TabIndex = 2;
            InputLabel.Text = "Исходные данные";
            InputLabel.Click += label1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { NameDGV, Like });
            dataGridView1.Location = new Point(12, 32);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(304, 404);
            dataGridView1.TabIndex = 3;
            // 
            // NameDGV
            // 
            NameDGV.HeaderText = "Название";
            NameDGV.MinimumWidth = 6;
            NameDGV.Name = "NameDGV";
            NameDGV.ReadOnly = true;
            NameDGV.Width = 125;
            // 
            // Like
            // 
            Like.HeaderText = "Лайк/Дизлайк";
            Like.MinimumWidth = 6;
            Like.Name = "Like";
            Like.Width = 125;
            // 
            // ResLabel
            // 
            ResLabel.AutoSize = true;
            ResLabel.Location = new Point(466, 9);
            ResLabel.Name = "ResLabel";
            ResLabel.Size = new Size(75, 20);
            ResLabel.TabIndex = 4;
            ResLabel.Text = "Результат";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(466, 32);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(322, 404);
            listBox1.TabIndex = 5;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(322, 372);
            button1.Name = "button1";
            button1.Size = new Size(138, 29);
            button1.TabIndex = 6;
            button1.Text = "Рекомендовать";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(322, 407);
            button2.Name = "button2";
            button2.Size = new Size(138, 29);
            button2.TabIndex = 7;
            button2.Text = "Сброс";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(322, 337);
            button3.Name = "button3";
            button3.Size = new Size(138, 29);
            button3.TabIndex = 8;
            button3.Text = "Фильтры";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(ResLabel);
            Controls.Add(dataGridView1);
            Controls.Add(InputLabel);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label InputLabel;
        public DataGridView dataGridView1;
        private DataGridViewTextBoxColumn NameDGV;
        private DataGridViewTextBoxColumn Like;
        private Label ResLabel;
        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
