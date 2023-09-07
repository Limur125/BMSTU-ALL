namespace TechUI
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
            this.authButton = new System.Windows.Forms.Button();
            this.authLoginTextbox = new System.Windows.Forms.TextBox();
            this.AuthPasswordTextbox = new System.Windows.Forms.TextBox();
            this.logoutButton = new System.Windows.Forms.Button();
            this.registerPasswordTextbox = new System.Windows.Forms.TextBox();
            this.registerLoginTextbox = new System.Windows.Forms.TextBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.LibraryButton = new System.Windows.Forms.Button();
            this.GetGameButton = new System.Windows.Forms.Button();
            this.GetGameTitleTextbox = new System.Windows.Forms.TextBox();
            this.SaveReviewButton = new System.Windows.Forms.Button();
            this.SaveReviewTitleTextbox = new System.Windows.Forms.TextBox();
            this.SaveReviewScoreNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SaveTimeRecordButton = new System.Windows.Forms.Button();
            this.SaveTimeRecordTypeCombobox = new System.Windows.Forms.ComboBox();
            this.SaveTimeRecordTitleTextbox = new System.Windows.Forms.TextBox();
            this.AddGameButton = new System.Windows.Forms.Button();
            this.AddGameTitleTextbox = new System.Windows.Forms.TextBox();
            this.AddGameReleaseDateCalendar = new System.Windows.Forms.MonthCalendar();
            this.AddGameDeveloperTextbox = new System.Windows.Forms.TextBox();
            this.AddGamePublisherTextbox = new System.Windows.Forms.TextBox();
            this.AddGamePlatformTextbox = new System.Windows.Forms.TextBox();
            this.SaveReviewTextTextbox = new System.Windows.Forms.TextBox();
            this.DeleteGameButton = new System.Windows.Forms.Button();
            this.DeleteGameTitleTextbox = new System.Windows.Forms.TextBox();
            this.UpdateGamePlatformTextbox = new System.Windows.Forms.TextBox();
            this.UpdateGamePublisherTextbox = new System.Windows.Forms.TextBox();
            this.UpdateGameDeveloperTextbox = new System.Windows.Forms.TextBox();
            this.UpdateGameReleaseDateCalendar = new System.Windows.Forms.MonthCalendar();
            this.UpdateGameTitleTextbox = new System.Windows.Forms.TextBox();
            this.UpdateGameButton = new System.Windows.Forms.Button();
            this.DeleteTimeRecordTitleTextbox = new System.Windows.Forms.TextBox();
            this.DeleteTimeRecordButton = new System.Windows.Forms.Button();
            this.DeleteReviewTitleTextbox = new System.Windows.Forms.TextBox();
            this.DeleteReviewButton = new System.Windows.Forms.Button();
            this.DeleteReviewUserTextbox = new System.Windows.Forms.TextBox();
            this.DeleteTimeRecordUserTextbox = new System.Windows.Forms.TextBox();
            this.SaveTimeRecordMinutesTextbox = new System.Windows.Forms.TextBox();
            this.SaveTimeRecordHoursTextbox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.SaveReviewScoreNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // authButton
            // 
            this.authButton.Location = new System.Drawing.Point(16, 12);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(119, 60);
            this.authButton.TabIndex = 0;
            this.authButton.Text = "Авторизация";
            this.authButton.UseVisualStyleBackColor = true;
            this.authButton.Click += new System.EventHandler(this.authButton_Click);
            // 
            // authLoginTextbox
            // 
            this.authLoginTextbox.Location = new System.Drawing.Point(141, 12);
            this.authLoginTextbox.Name = "authLoginTextbox";
            this.authLoginTextbox.PlaceholderText = "Логин";
            this.authLoginTextbox.Size = new System.Drawing.Size(125, 27);
            this.authLoginTextbox.TabIndex = 1;
            // 
            // AuthPasswordTextbox
            // 
            this.AuthPasswordTextbox.Location = new System.Drawing.Point(141, 45);
            this.AuthPasswordTextbox.Name = "AuthPasswordTextbox";
            this.AuthPasswordTextbox.PlaceholderText = "Пароль";
            this.AuthPasswordTextbox.Size = new System.Drawing.Size(125, 27);
            this.AuthPasswordTextbox.TabIndex = 2;
            // 
            // logoutButton
            // 
            this.logoutButton.Enabled = false;
            this.logoutButton.Location = new System.Drawing.Point(16, 78);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(119, 60);
            this.logoutButton.TabIndex = 3;
            this.logoutButton.Text = "Выйти из аккаунта";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // registerPasswordTextbox
            // 
            this.registerPasswordTextbox.Location = new System.Drawing.Point(141, 177);
            this.registerPasswordTextbox.Name = "registerPasswordTextbox";
            this.registerPasswordTextbox.PlaceholderText = "Пароль";
            this.registerPasswordTextbox.Size = new System.Drawing.Size(125, 27);
            this.registerPasswordTextbox.TabIndex = 6;
            // 
            // registerLoginTextbox
            // 
            this.registerLoginTextbox.Location = new System.Drawing.Point(141, 144);
            this.registerLoginTextbox.Name = "registerLoginTextbox";
            this.registerLoginTextbox.PlaceholderText = "Логин";
            this.registerLoginTextbox.Size = new System.Drawing.Size(125, 27);
            this.registerLoginTextbox.TabIndex = 5;
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(16, 144);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(119, 60);
            this.registerButton.TabIndex = 4;
            this.registerButton.Text = "Регистрация";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // LibraryButton
            // 
            this.LibraryButton.Location = new System.Drawing.Point(16, 210);
            this.LibraryButton.Name = "LibraryButton";
            this.LibraryButton.Size = new System.Drawing.Size(119, 60);
            this.LibraryButton.TabIndex = 7;
            this.LibraryButton.Text = "Получить библитеку";
            this.LibraryButton.UseVisualStyleBackColor = true;
            this.LibraryButton.Click += new System.EventHandler(this.LibraryButton_Click);
            // 
            // GetGameButton
            // 
            this.GetGameButton.Location = new System.Drawing.Point(16, 276);
            this.GetGameButton.Name = "GetGameButton";
            this.GetGameButton.Size = new System.Drawing.Size(119, 70);
            this.GetGameButton.TabIndex = 8;
            this.GetGameButton.Text = "Получить страницу игры";
            this.GetGameButton.UseVisualStyleBackColor = true;
            this.GetGameButton.Click += new System.EventHandler(this.GetGameButton_Click);
            // 
            // GetGameTitleTextbox
            // 
            this.GetGameTitleTextbox.Location = new System.Drawing.Point(141, 276);
            this.GetGameTitleTextbox.Name = "GetGameTitleTextbox";
            this.GetGameTitleTextbox.PlaceholderText = "Название игры";
            this.GetGameTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.GetGameTitleTextbox.TabIndex = 9;
            // 
            // SaveReviewButton
            // 
            this.SaveReviewButton.Enabled = false;
            this.SaveReviewButton.Location = new System.Drawing.Point(16, 352);
            this.SaveReviewButton.Name = "SaveReviewButton";
            this.SaveReviewButton.Size = new System.Drawing.Size(119, 60);
            this.SaveReviewButton.TabIndex = 10;
            this.SaveReviewButton.Text = "Добавить отзыв об игре";
            this.SaveReviewButton.UseVisualStyleBackColor = true;
            this.SaveReviewButton.Click += new System.EventHandler(this.SaveReviewButton_Click);
            // 
            // SaveReviewTitleTextbox
            // 
            this.SaveReviewTitleTextbox.Location = new System.Drawing.Point(141, 352);
            this.SaveReviewTitleTextbox.Name = "SaveReviewTitleTextbox";
            this.SaveReviewTitleTextbox.PlaceholderText = "Название игры";
            this.SaveReviewTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.SaveReviewTitleTextbox.TabIndex = 11;
            // 
            // SaveReviewScoreNumericUpDown
            // 
            this.SaveReviewScoreNumericUpDown.Location = new System.Drawing.Point(272, 353);
            this.SaveReviewScoreNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SaveReviewScoreNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SaveReviewScoreNumericUpDown.Name = "SaveReviewScoreNumericUpDown";
            this.SaveReviewScoreNumericUpDown.Size = new System.Drawing.Size(150, 27);
            this.SaveReviewScoreNumericUpDown.TabIndex = 13;
            this.SaveReviewScoreNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SaveTimeRecordButton
            // 
            this.SaveTimeRecordButton.Enabled = false;
            this.SaveTimeRecordButton.Location = new System.Drawing.Point(16, 597);
            this.SaveTimeRecordButton.Name = "SaveTimeRecordButton";
            this.SaveTimeRecordButton.Size = new System.Drawing.Size(119, 92);
            this.SaveTimeRecordButton.TabIndex = 14;
            this.SaveTimeRecordButton.Text = "Добавить игре временную отметку";
            this.SaveTimeRecordButton.UseVisualStyleBackColor = true;
            this.SaveTimeRecordButton.Click += new System.EventHandler(this.SaveTimeRecordButton_Click);
            // 
            // SaveTimeRecordTypeCombobox
            // 
            this.SaveTimeRecordTypeCombobox.FormattingEnabled = true;
            this.SaveTimeRecordTypeCombobox.Items.AddRange(new object[] {
            "Обычное прохождение",
            "Полное прохождение"});
            this.SaveTimeRecordTypeCombobox.Location = new System.Drawing.Point(141, 601);
            this.SaveTimeRecordTypeCombobox.Name = "SaveTimeRecordTypeCombobox";
            this.SaveTimeRecordTypeCombobox.Size = new System.Drawing.Size(151, 28);
            this.SaveTimeRecordTypeCombobox.TabIndex = 15;
            // 
            // SaveTimeRecordTitleTextbox
            // 
            this.SaveTimeRecordTitleTextbox.Location = new System.Drawing.Point(297, 601);
            this.SaveTimeRecordTitleTextbox.Name = "SaveTimeRecordTitleTextbox";
            this.SaveTimeRecordTitleTextbox.PlaceholderText = "Название игры";
            this.SaveTimeRecordTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.SaveTimeRecordTitleTextbox.TabIndex = 16;
            // 
            // AddGameButton
            // 
            this.AddGameButton.Enabled = false;
            this.AddGameButton.Location = new System.Drawing.Point(489, 12);
            this.AddGameButton.Name = "AddGameButton";
            this.AddGameButton.Size = new System.Drawing.Size(119, 70);
            this.AddGameButton.TabIndex = 18;
            this.AddGameButton.Text = "Добавить игру";
            this.AddGameButton.UseVisualStyleBackColor = true;
            this.AddGameButton.Click += new System.EventHandler(this.AddGameButton_Click);
            // 
            // AddGameTitleTextbox
            // 
            this.AddGameTitleTextbox.Location = new System.Drawing.Point(614, 12);
            this.AddGameTitleTextbox.Name = "AddGameTitleTextbox";
            this.AddGameTitleTextbox.PlaceholderText = "Название игры";
            this.AddGameTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.AddGameTitleTextbox.TabIndex = 19;
            // 
            // AddGameReleaseDateCalendar
            // 
            this.AddGameReleaseDateCalendar.Location = new System.Drawing.Point(751, 12);
            this.AddGameReleaseDateCalendar.Name = "AddGameReleaseDateCalendar";
            this.AddGameReleaseDateCalendar.TabIndex = 20;
            // 
            // AddGameDeveloperTextbox
            // 
            this.AddGameDeveloperTextbox.Location = new System.Drawing.Point(614, 45);
            this.AddGameDeveloperTextbox.Name = "AddGameDeveloperTextbox";
            this.AddGameDeveloperTextbox.PlaceholderText = "Разработчик";
            this.AddGameDeveloperTextbox.Size = new System.Drawing.Size(125, 27);
            this.AddGameDeveloperTextbox.TabIndex = 21;
            // 
            // AddGamePublisherTextbox
            // 
            this.AddGamePublisherTextbox.Location = new System.Drawing.Point(614, 78);
            this.AddGamePublisherTextbox.Name = "AddGamePublisherTextbox";
            this.AddGamePublisherTextbox.PlaceholderText = "Издатель";
            this.AddGamePublisherTextbox.Size = new System.Drawing.Size(125, 27);
            this.AddGamePublisherTextbox.TabIndex = 22;
            // 
            // AddGamePlatformTextbox
            // 
            this.AddGamePlatformTextbox.Location = new System.Drawing.Point(614, 111);
            this.AddGamePlatformTextbox.Name = "AddGamePlatformTextbox";
            this.AddGamePlatformTextbox.PlaceholderText = "Платформы";
            this.AddGamePlatformTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AddGamePlatformTextbox.Size = new System.Drawing.Size(125, 27);
            this.AddGamePlatformTextbox.TabIndex = 23;
            // 
            // SaveReviewTextTextbox
            // 
            this.SaveReviewTextTextbox.Location = new System.Drawing.Point(141, 385);
            this.SaveReviewTextTextbox.Multiline = true;
            this.SaveReviewTextTextbox.Name = "SaveReviewTextTextbox";
            this.SaveReviewTextTextbox.PlaceholderText = "Текст отзыва";
            this.SaveReviewTextTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SaveReviewTextTextbox.Size = new System.Drawing.Size(281, 210);
            this.SaveReviewTextTextbox.TabIndex = 24;
            // 
            // DeleteGameButton
            // 
            this.DeleteGameButton.Enabled = false;
            this.DeleteGameButton.Location = new System.Drawing.Point(489, 223);
            this.DeleteGameButton.Name = "DeleteGameButton";
            this.DeleteGameButton.Size = new System.Drawing.Size(119, 70);
            this.DeleteGameButton.TabIndex = 25;
            this.DeleteGameButton.Text = "Удалить игру";
            this.DeleteGameButton.UseVisualStyleBackColor = true;
            this.DeleteGameButton.Click += new System.EventHandler(this.DeleteGameButton_Click);
            // 
            // DeleteGameTitleTextbox
            // 
            this.DeleteGameTitleTextbox.Location = new System.Drawing.Point(614, 227);
            this.DeleteGameTitleTextbox.Name = "DeleteGameTitleTextbox";
            this.DeleteGameTitleTextbox.PlaceholderText = "Название игры";
            this.DeleteGameTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.DeleteGameTitleTextbox.TabIndex = 26;
            // 
            // UpdateGamePlatformTextbox
            // 
            this.UpdateGamePlatformTextbox.Location = new System.Drawing.Point(1080, 111);
            this.UpdateGamePlatformTextbox.Multiline = true;
            this.UpdateGamePlatformTextbox.Name = "UpdateGamePlatformTextbox";
            this.UpdateGamePlatformTextbox.PlaceholderText = "Платформы";
            this.UpdateGamePlatformTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UpdateGamePlatformTextbox.Size = new System.Drawing.Size(125, 108);
            this.UpdateGamePlatformTextbox.TabIndex = 32;
            // 
            // UpdateGamePublisherTextbox
            // 
            this.UpdateGamePublisherTextbox.Location = new System.Drawing.Point(1080, 78);
            this.UpdateGamePublisherTextbox.Name = "UpdateGamePublisherTextbox";
            this.UpdateGamePublisherTextbox.PlaceholderText = "Издатель";
            this.UpdateGamePublisherTextbox.Size = new System.Drawing.Size(125, 27);
            this.UpdateGamePublisherTextbox.TabIndex = 31;
            // 
            // UpdateGameDeveloperTextbox
            // 
            this.UpdateGameDeveloperTextbox.Location = new System.Drawing.Point(1080, 45);
            this.UpdateGameDeveloperTextbox.Name = "UpdateGameDeveloperTextbox";
            this.UpdateGameDeveloperTextbox.PlaceholderText = "Разработчик";
            this.UpdateGameDeveloperTextbox.Size = new System.Drawing.Size(125, 27);
            this.UpdateGameDeveloperTextbox.TabIndex = 30;
            // 
            // UpdateGameReleaseDateCalendar
            // 
            this.UpdateGameReleaseDateCalendar.Location = new System.Drawing.Point(1217, 12);
            this.UpdateGameReleaseDateCalendar.Name = "UpdateGameReleaseDateCalendar";
            this.UpdateGameReleaseDateCalendar.TabIndex = 29;
            // 
            // UpdateGameTitleTextbox
            // 
            this.UpdateGameTitleTextbox.Location = new System.Drawing.Point(1080, 12);
            this.UpdateGameTitleTextbox.Name = "UpdateGameTitleTextbox";
            this.UpdateGameTitleTextbox.PlaceholderText = "Название игры";
            this.UpdateGameTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.UpdateGameTitleTextbox.TabIndex = 28;
            // 
            // UpdateGameButton
            // 
            this.UpdateGameButton.Enabled = false;
            this.UpdateGameButton.Location = new System.Drawing.Point(955, 12);
            this.UpdateGameButton.Name = "UpdateGameButton";
            this.UpdateGameButton.Size = new System.Drawing.Size(119, 70);
            this.UpdateGameButton.TabIndex = 27;
            this.UpdateGameButton.Text = "Изменить игру";
            this.UpdateGameButton.UseVisualStyleBackColor = true;
            this.UpdateGameButton.Click += new System.EventHandler(this.UpdateGameButton_Click);
            // 
            // DeleteTimeRecordTitleTextbox
            // 
            this.DeleteTimeRecordTitleTextbox.Location = new System.Drawing.Point(553, 602);
            this.DeleteTimeRecordTitleTextbox.Name = "DeleteTimeRecordTitleTextbox";
            this.DeleteTimeRecordTitleTextbox.PlaceholderText = "Название игры";
            this.DeleteTimeRecordTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.DeleteTimeRecordTitleTextbox.TabIndex = 38;
            // 
            // DeleteTimeRecordButton
            // 
            this.DeleteTimeRecordButton.Enabled = false;
            this.DeleteTimeRecordButton.Location = new System.Drawing.Point(428, 598);
            this.DeleteTimeRecordButton.Name = "DeleteTimeRecordButton";
            this.DeleteTimeRecordButton.Size = new System.Drawing.Size(119, 92);
            this.DeleteTimeRecordButton.TabIndex = 36;
            this.DeleteTimeRecordButton.Text = "Удалить у игры временную отметку";
            this.DeleteTimeRecordButton.UseVisualStyleBackColor = true;
            this.DeleteTimeRecordButton.Click += new System.EventHandler(this.DeleteTimeRecordButton_Click);
            // 
            // DeleteReviewTitleTextbox
            // 
            this.DeleteReviewTitleTextbox.Location = new System.Drawing.Point(553, 353);
            this.DeleteReviewTitleTextbox.Name = "DeleteReviewTitleTextbox";
            this.DeleteReviewTitleTextbox.PlaceholderText = "Название игры";
            this.DeleteReviewTitleTextbox.Size = new System.Drawing.Size(125, 27);
            this.DeleteReviewTitleTextbox.TabIndex = 34;
            // 
            // DeleteReviewButton
            // 
            this.DeleteReviewButton.Enabled = false;
            this.DeleteReviewButton.Location = new System.Drawing.Point(428, 353);
            this.DeleteReviewButton.Name = "DeleteReviewButton";
            this.DeleteReviewButton.Size = new System.Drawing.Size(119, 60);
            this.DeleteReviewButton.TabIndex = 33;
            this.DeleteReviewButton.Text = "Удалить отзыв об игре";
            this.DeleteReviewButton.UseVisualStyleBackColor = true;
            this.DeleteReviewButton.Click += new System.EventHandler(this.DeleteReviewButton_Click);
            // 
            // DeleteReviewUserTextbox
            // 
            this.DeleteReviewUserTextbox.Location = new System.Drawing.Point(553, 385);
            this.DeleteReviewUserTextbox.Name = "DeleteReviewUserTextbox";
            this.DeleteReviewUserTextbox.PlaceholderText = "Имя пользователя";
            this.DeleteReviewUserTextbox.Size = new System.Drawing.Size(125, 27);
            this.DeleteReviewUserTextbox.TabIndex = 39;
            // 
            // DeleteTimeRecordUserTextbox
            // 
            this.DeleteTimeRecordUserTextbox.Location = new System.Drawing.Point(553, 635);
            this.DeleteTimeRecordUserTextbox.Name = "DeleteTimeRecordUserTextbox";
            this.DeleteTimeRecordUserTextbox.PlaceholderText = "Имя пользователя";
            this.DeleteTimeRecordUserTextbox.Size = new System.Drawing.Size(125, 27);
            this.DeleteTimeRecordUserTextbox.TabIndex = 40;
            // 
            // SaveTimeRecordMinutesTextbox
            // 
            this.SaveTimeRecordMinutesTextbox.Location = new System.Drawing.Point(297, 635);
            this.SaveTimeRecordMinutesTextbox.Name = "SaveTimeRecordMinutesTextbox";
            this.SaveTimeRecordMinutesTextbox.PlaceholderText = "Минуты";
            this.SaveTimeRecordMinutesTextbox.Size = new System.Drawing.Size(125, 27);
            this.SaveTimeRecordMinutesTextbox.TabIndex = 42;
            // 
            // SaveTimeRecordHoursTextbox
            // 
            this.SaveTimeRecordHoursTextbox.Location = new System.Drawing.Point(141, 635);
            this.SaveTimeRecordHoursTextbox.Name = "SaveTimeRecordHoursTextbox";
            this.SaveTimeRecordHoursTextbox.PlaceholderText = "Часы";
            this.SaveTimeRecordHoursTextbox.Size = new System.Drawing.Size(125, 27);
            this.SaveTimeRecordHoursTextbox.TabIndex = 41;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.SaveTimeRecordMinutesTextbox);
            this.Controls.Add(this.SaveTimeRecordHoursTextbox);
            this.Controls.Add(this.DeleteTimeRecordUserTextbox);
            this.Controls.Add(this.DeleteReviewUserTextbox);
            this.Controls.Add(this.DeleteTimeRecordTitleTextbox);
            this.Controls.Add(this.DeleteTimeRecordButton);
            this.Controls.Add(this.DeleteReviewTitleTextbox);
            this.Controls.Add(this.DeleteReviewButton);
            this.Controls.Add(this.UpdateGamePlatformTextbox);
            this.Controls.Add(this.UpdateGamePublisherTextbox);
            this.Controls.Add(this.UpdateGameDeveloperTextbox);
            this.Controls.Add(this.UpdateGameReleaseDateCalendar);
            this.Controls.Add(this.UpdateGameTitleTextbox);
            this.Controls.Add(this.UpdateGameButton);
            this.Controls.Add(this.DeleteGameTitleTextbox);
            this.Controls.Add(this.DeleteGameButton);
            this.Controls.Add(this.SaveReviewTextTextbox);
            this.Controls.Add(this.AddGamePlatformTextbox);
            this.Controls.Add(this.AddGamePublisherTextbox);
            this.Controls.Add(this.AddGameDeveloperTextbox);
            this.Controls.Add(this.AddGameReleaseDateCalendar);
            this.Controls.Add(this.AddGameTitleTextbox);
            this.Controls.Add(this.AddGameButton);
            this.Controls.Add(this.SaveTimeRecordTitleTextbox);
            this.Controls.Add(this.SaveTimeRecordTypeCombobox);
            this.Controls.Add(this.SaveTimeRecordButton);
            this.Controls.Add(this.SaveReviewScoreNumericUpDown);
            this.Controls.Add(this.SaveReviewTitleTextbox);
            this.Controls.Add(this.SaveReviewButton);
            this.Controls.Add(this.GetGameTitleTextbox);
            this.Controls.Add(this.GetGameButton);
            this.Controls.Add(this.LibraryButton);
            this.Controls.Add(this.registerPasswordTextbox);
            this.Controls.Add(this.registerLoginTextbox);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.AuthPasswordTextbox);
            this.Controls.Add(this.authLoginTextbox);
            this.Controls.Add(this.authButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SaveReviewScoreNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button authButton;
        private TextBox authLoginTextbox;
        private TextBox AuthPasswordTextbox;
        private Button logoutButton;
        private TextBox registerPasswordTextbox;
        private TextBox registerLoginTextbox;
        private Button registerButton;
        private Button LibraryButton;
        private Button GetGameButton;
        private TextBox GetGameTitleTextbox;
        private Button SaveReviewButton;
        private TextBox SaveReviewTitleTextbox;
        private NumericUpDown SaveReviewScoreNumericUpDown;
        private Button SaveTimeRecordButton;
        private ComboBox SaveTimeRecordTypeCombobox;
        private TextBox SaveTimeRecordTitleTextbox;
        private Button AddGameButton;
        private TextBox AddGameTitleTextbox;
        private MonthCalendar AddGameReleaseDateCalendar;
        private TextBox AddGameDeveloperTextbox;
        private TextBox AddGamePublisherTextbox;
        private TextBox AddGamePlatformTextbox;
        private TextBox SaveReviewTextTextbox;
        private Button DeleteGameButton;
        private TextBox DeleteGameTitleTextbox;
        private TextBox UpdateGamePlatformTextbox;
        private TextBox UpdateGamePublisherTextbox;
        private TextBox UpdateGameDeveloperTextbox;
        private MonthCalendar UpdateGameReleaseDateCalendar;
        private TextBox UpdateGameTitleTextbox;
        private Button UpdateGameButton;
        private TextBox DeleteTimeRecordTitleTextbox;
        private Button DeleteTimeRecordButton;
        private TextBox DeleteReviewTitleTextbox;
        private Button DeleteReviewButton;
        private TextBox DeleteReviewUserTextbox;
        private TextBox DeleteTimeRecordUserTextbox;
        private TextBox SaveTimeRecordMinutesTextbox;
        private TextBox SaveTimeRecordHoursTextbox;
    }
}