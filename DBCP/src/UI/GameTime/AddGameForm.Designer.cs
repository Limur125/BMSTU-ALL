namespace GameTime
{
    partial class AddGameForm
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
            this.GameButton = new System.Windows.Forms.Button();
            this.GameTitle = new System.Windows.Forms.TextBox();
            this.GameDeveloper = new System.Windows.Forms.TextBox();
            this.GamePublisher = new System.Windows.Forms.TextBox();
            this.GamePlatform = new System.Windows.Forms.TextBox();
            this.GameReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // GameButton
            // 
            this.GameButton.Location = new System.Drawing.Point(12, 177);
            this.GameButton.Name = "GameButton";
            this.GameButton.Size = new System.Drawing.Size(776, 61);
            this.GameButton.TabIndex = 0;
            this.GameButton.Text = "Добавить игру";
            this.GameButton.UseVisualStyleBackColor = true;
            this.GameButton.Click += new System.EventHandler(this.GameButton_Click);
            // 
            // GameTitle
            // 
            this.GameTitle.Location = new System.Drawing.Point(12, 12);
            this.GameTitle.Name = "GameTitle";
            this.GameTitle.PlaceholderText = "Название";
            this.GameTitle.Size = new System.Drawing.Size(776, 27);
            this.GameTitle.TabIndex = 2;
            // 
            // GameDeveloper
            // 
            this.GameDeveloper.Location = new System.Drawing.Point(12, 45);
            this.GameDeveloper.Name = "GameDeveloper";
            this.GameDeveloper.PlaceholderText = "Разработчик";
            this.GameDeveloper.Size = new System.Drawing.Size(776, 27);
            this.GameDeveloper.TabIndex = 3;
            // 
            // GamePublisher
            // 
            this.GamePublisher.Location = new System.Drawing.Point(12, 78);
            this.GamePublisher.Name = "GamePublisher";
            this.GamePublisher.PlaceholderText = "Издатель";
            this.GamePublisher.Size = new System.Drawing.Size(776, 27);
            this.GamePublisher.TabIndex = 4;
            // 
            // GamePlatform
            // 
            this.GamePlatform.Location = new System.Drawing.Point(12, 111);
            this.GamePlatform.Name = "GamePlatform";
            this.GamePlatform.PlaceholderText = "Жанры";
            this.GamePlatform.Size = new System.Drawing.Size(776, 27);
            this.GamePlatform.TabIndex = 5;
            // 
            // GameReleaseDate
            // 
            this.GameReleaseDate.Location = new System.Drawing.Point(12, 144);
            this.GameReleaseDate.Name = "GameReleaseDate";
            this.GameReleaseDate.Size = new System.Drawing.Size(776, 27);
            this.GameReleaseDate.TabIndex = 6;
            // 
            // AddGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 249);
            this.Controls.Add(this.GameReleaseDate);
            this.Controls.Add(this.GamePlatform);
            this.Controls.Add(this.GamePublisher);
            this.Controls.Add(this.GameDeveloper);
            this.Controls.Add(this.GameTitle);
            this.Controls.Add(this.GameButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddGameForm";
            this.Text = "Добавить игру";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button GameButton;
        private TextBox GameTitle;
        private TextBox GameDeveloper;
        private TextBox GamePublisher;
        private TextBox GamePlatform;
        private DateTimePicker GameReleaseDate;
    }
}