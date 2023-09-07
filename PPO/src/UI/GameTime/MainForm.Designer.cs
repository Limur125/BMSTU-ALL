namespace GameTime
{
    partial class MainForm
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
        private void AddLibraryButtons()
        {
            this.libraryButtons = new List<Button>();
            List<string> titles = GetLibrary();
            this.LibraryGroupbox.SuspendLayout();
            int i = 0;
            vScrollBar1.Maximum = titles.Count;
            foreach (string title in titles)
            {
                Button b = new Button();
                LibraryGroupbox.Controls.Add(b);
                b.Location = new System.Drawing.Point(6, 26 + i * 78);
                b.Name = $"{title}Button";
                b.Size = new System.Drawing.Size(868, 72);
                b.TabIndex = i;
                b.Text = title;
                b.UseVisualStyleBackColor = true;
                b.Click += new System.EventHandler(this.SelectGame);
                libraryButtons.Add(b);
                i++;
            }
            this.LibraryGroupbox.ResumeLayout(false);
            this.LibraryGroupbox.PerformLayout();
        }

        void RefreshLibraryButtons()
        {
            foreach (var button in libraryButtons)
                button.Dispose();
            AddLibraryButtons();
        }
        
        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LibraryGroupbox = new System.Windows.Forms.GroupBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.GamePage = new System.Windows.Forms.GroupBox();
            this.Score = new System.Windows.Forms.Label();
            this.AllTimeRecords = new System.Windows.Forms.Button();
            this.AddTimeRecordButton = new System.Windows.Forms.Button();
            this.GPReview4Score = new System.Windows.Forms.Label();
            this.GPReview3Score = new System.Windows.Forms.Label();
            this.GPReview2Score = new System.Windows.Forms.Label();
            this.GPReview1Score = new System.Windows.Forms.Label();
            this.UpdateGameButton = new System.Windows.Forms.Button();
            this.DeleteGameButton = new System.Windows.Forms.Button();
            this.AllReviewsButton = new System.Windows.Forms.Button();
            this.AddReviewButton = new System.Windows.Forms.Button();
            this.GPReview4Text = new System.Windows.Forms.TextBox();
            this.GPReview4Date = new System.Windows.Forms.Label();
            this.GPReview4User = new System.Windows.Forms.Label();
            this.GPReview3Text = new System.Windows.Forms.TextBox();
            this.GPReview3Date = new System.Windows.Forms.Label();
            this.GPReview3User = new System.Windows.Forms.Label();
            this.GPReview2Text = new System.Windows.Forms.TextBox();
            this.GPReview2Date = new System.Windows.Forms.Label();
            this.GPReview2User = new System.Windows.Forms.Label();
            this.GPReview1Text = new System.Windows.Forms.TextBox();
            this.GPReview1Date = new System.Windows.Forms.Label();
            this.GPReview1User = new System.Windows.Forms.Label();
            this.GamePageFullCount = new System.Windows.Forms.Label();
            this.GamePageFullMax = new System.Windows.Forms.Label();
            this.GamePageFullMid = new System.Windows.Forms.Label();
            this.GamePageFullMin = new System.Windows.Forms.Label();
            this.GamePageFullAvg = new System.Windows.Forms.Label();
            this.GamePageNormalCount = new System.Windows.Forms.Label();
            this.GamePageNormalMin = new System.Windows.Forms.Label();
            this.GamePageNormalMax = new System.Windows.Forms.Label();
            this.GamePageNormalMid = new System.Windows.Forms.Label();
            this.GamePageNormalAvg = new System.Windows.Forms.Label();
            this.GamePageCount = new System.Windows.Forms.Label();
            this.BackButton = new System.Windows.Forms.Button();
            this.GamePageScore = new System.Windows.Forms.Label();
            this.GamePageFull = new System.Windows.Forms.Label();
            this.GamePageMin = new System.Windows.Forms.Label();
            this.GamePageMax = new System.Windows.Forms.Label();
            this.GamePageMid = new System.Windows.Forms.Label();
            this.GamePageAvg = new System.Windows.Forms.Label();
            this.GamePageNormal = new System.Windows.Forms.Label();
            this.GamePageTimeStat = new System.Windows.Forms.Label();
            this.GamePagePlatform = new System.Windows.Forms.Label();
            this.GamePagePublisher = new System.Windows.Forms.Label();
            this.GamePageDeveloper = new System.Windows.Forms.Label();
            this.GamePageReleaseDate = new System.Windows.Forms.Label();
            this.GamePageTitle = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.AuthButton = new System.Windows.Forms.Button();
            this.LogoutButton = new System.Windows.Forms.Button();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginMessage = new System.Windows.Forms.Label();
            this.AddGameButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LibraryGroupbox.SuspendLayout();
            this.GamePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // LibraryGroupbox
            // 
            this.LibraryGroupbox.Controls.Add(this.vScrollBar1);
            this.LibraryGroupbox.Location = new System.Drawing.Point(12, 43);
            this.LibraryGroupbox.Name = "LibraryGroupbox";
            this.LibraryGroupbox.Size = new System.Drawing.Size(958, 898);
            this.LibraryGroupbox.TabIndex = 0;
            this.LibraryGroupbox.TabStop = false;
            this.LibraryGroupbox.Text = "Библиотека игр";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(926, 16);
            this.vScrollBar1.Maximum = 10;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(26, 876);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // GamePage
            // 
            this.GamePage.Controls.Add(this.label5);
            this.GamePage.Controls.Add(this.label4);
            this.GamePage.Controls.Add(this.label3);
            this.GamePage.Controls.Add(this.label2);
            this.GamePage.Controls.Add(this.label1);
            this.GamePage.Controls.Add(this.Score);
            this.GamePage.Controls.Add(this.AllTimeRecords);
            this.GamePage.Controls.Add(this.AddTimeRecordButton);
            this.GamePage.Controls.Add(this.GPReview4Score);
            this.GamePage.Controls.Add(this.GPReview3Score);
            this.GamePage.Controls.Add(this.GPReview2Score);
            this.GamePage.Controls.Add(this.GPReview1Score);
            this.GamePage.Controls.Add(this.UpdateGameButton);
            this.GamePage.Controls.Add(this.DeleteGameButton);
            this.GamePage.Controls.Add(this.AllReviewsButton);
            this.GamePage.Controls.Add(this.AddReviewButton);
            this.GamePage.Controls.Add(this.GPReview4Text);
            this.GamePage.Controls.Add(this.GPReview4Date);
            this.GamePage.Controls.Add(this.GPReview4User);
            this.GamePage.Controls.Add(this.GPReview3Text);
            this.GamePage.Controls.Add(this.GPReview3Date);
            this.GamePage.Controls.Add(this.GPReview3User);
            this.GamePage.Controls.Add(this.GPReview2Text);
            this.GamePage.Controls.Add(this.GPReview2Date);
            this.GamePage.Controls.Add(this.GPReview2User);
            this.GamePage.Controls.Add(this.GPReview1Text);
            this.GamePage.Controls.Add(this.GPReview1Date);
            this.GamePage.Controls.Add(this.GPReview1User);
            this.GamePage.Controls.Add(this.GamePageFullCount);
            this.GamePage.Controls.Add(this.GamePageFullMax);
            this.GamePage.Controls.Add(this.GamePageFullMid);
            this.GamePage.Controls.Add(this.GamePageFullMin);
            this.GamePage.Controls.Add(this.GamePageFullAvg);
            this.GamePage.Controls.Add(this.GamePageNormalCount);
            this.GamePage.Controls.Add(this.GamePageNormalMin);
            this.GamePage.Controls.Add(this.GamePageNormalMax);
            this.GamePage.Controls.Add(this.GamePageNormalMid);
            this.GamePage.Controls.Add(this.GamePageNormalAvg);
            this.GamePage.Controls.Add(this.GamePageCount);
            this.GamePage.Controls.Add(this.BackButton);
            this.GamePage.Controls.Add(this.GamePageScore);
            this.GamePage.Controls.Add(this.GamePageFull);
            this.GamePage.Controls.Add(this.GamePageMin);
            this.GamePage.Controls.Add(this.GamePageMax);
            this.GamePage.Controls.Add(this.GamePageMid);
            this.GamePage.Controls.Add(this.GamePageAvg);
            this.GamePage.Controls.Add(this.GamePageNormal);
            this.GamePage.Controls.Add(this.GamePageTimeStat);
            this.GamePage.Controls.Add(this.GamePagePlatform);
            this.GamePage.Controls.Add(this.GamePagePublisher);
            this.GamePage.Controls.Add(this.GamePageDeveloper);
            this.GamePage.Controls.Add(this.GamePageReleaseDate);
            this.GamePage.Controls.Add(this.GamePageTitle);
            this.GamePage.Location = new System.Drawing.Point(12, 43);
            this.GamePage.Name = "GamePage";
            this.GamePage.Size = new System.Drawing.Size(958, 898);
            this.GamePage.TabIndex = 0;
            this.GamePage.TabStop = false;
            // 
            // Score
            // 
            this.Score.AutoSize = true;
            this.Score.Location = new System.Drawing.Point(634, 246);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(122, 20);
            this.Score.TabIndex = 46;
            this.Score.Text = "Средняя оценка";
            // 
            // AllTimeRecords
            // 
            this.AllTimeRecords.Location = new System.Drawing.Point(503, 838);
            this.AllTimeRecords.Name = "AllTimeRecords";
            this.AllTimeRecords.Size = new System.Drawing.Size(238, 54);
            this.AllTimeRecords.TabIndex = 45;
            this.AllTimeRecords.Text = "Посмотреть все временные отметки";
            this.AllTimeRecords.UseVisualStyleBackColor = true;
            this.AllTimeRecords.Click += new System.EventHandler(this.AllTimeRecords_Click);
            // 
            // AddTimeRecordButton
            // 
            this.AddTimeRecordButton.Location = new System.Drawing.Point(747, 838);
            this.AddTimeRecordButton.Name = "AddTimeRecordButton";
            this.AddTimeRecordButton.Size = new System.Drawing.Size(205, 54);
            this.AddTimeRecordButton.TabIndex = 44;
            this.AddTimeRecordButton.Text = "Добавить время";
            this.AddTimeRecordButton.UseVisualStyleBackColor = true;
            this.AddTimeRecordButton.Click += new System.EventHandler(this.AddTimeRecordButton_Click);
            // 
            // GPReview4Score
            // 
            this.GPReview4Score.AutoSize = true;
            this.GPReview4Score.Location = new System.Drawing.Point(749, 643);
            this.GPReview4Score.Name = "GPReview4Score";
            this.GPReview4Score.Size = new System.Drawing.Size(101, 20);
            this.GPReview4Score.TabIndex = 43;
            this.GPReview4Score.Text = "Review4Score";
            // 
            // GPReview3Score
            // 
            this.GPReview3Score.AutoSize = true;
            this.GPReview3Score.Location = new System.Drawing.Point(257, 642);
            this.GPReview3Score.Name = "GPReview3Score";
            this.GPReview3Score.Size = new System.Drawing.Size(101, 20);
            this.GPReview3Score.TabIndex = 42;
            this.GPReview3Score.Text = "Review3Score";
            // 
            // GPReview2Score
            // 
            this.GPReview2Score.AutoSize = true;
            this.GPReview2Score.Location = new System.Drawing.Point(749, 428);
            this.GPReview2Score.Name = "GPReview2Score";
            this.GPReview2Score.Size = new System.Drawing.Size(101, 20);
            this.GPReview2Score.TabIndex = 41;
            this.GPReview2Score.Text = "Review2Score";
            // 
            // GPReview1Score
            // 
            this.GPReview1Score.AutoSize = true;
            this.GPReview1Score.Location = new System.Drawing.Point(257, 428);
            this.GPReview1Score.Name = "GPReview1Score";
            this.GPReview1Score.Size = new System.Drawing.Size(101, 20);
            this.GPReview1Score.TabIndex = 40;
            this.GPReview1Score.Text = "Review1Score";
            // 
            // UpdateGameButton
            // 
            this.UpdateGameButton.Location = new System.Drawing.Point(816, 70);
            this.UpdateGameButton.Name = "UpdateGameButton";
            this.UpdateGameButton.Size = new System.Drawing.Size(136, 48);
            this.UpdateGameButton.TabIndex = 39;
            this.UpdateGameButton.Text = "Изменить игру";
            this.UpdateGameButton.UseVisualStyleBackColor = true;
            this.UpdateGameButton.Click += new System.EventHandler(this.UpdateGameButton_Click);
            // 
            // DeleteGameButton
            // 
            this.DeleteGameButton.Location = new System.Drawing.Point(816, 16);
            this.DeleteGameButton.Name = "DeleteGameButton";
            this.DeleteGameButton.Size = new System.Drawing.Size(136, 48);
            this.DeleteGameButton.TabIndex = 38;
            this.DeleteGameButton.Text = "Удалить игру";
            this.DeleteGameButton.UseVisualStyleBackColor = true;
            this.DeleteGameButton.Click += new System.EventHandler(this.DeleteGameButton_Click);
            // 
            // AllReviewsButton
            // 
            this.AllReviewsButton.Location = new System.Drawing.Point(217, 838);
            this.AllReviewsButton.Name = "AllReviewsButton";
            this.AllReviewsButton.Size = new System.Drawing.Size(238, 54);
            this.AllReviewsButton.TabIndex = 37;
            this.AllReviewsButton.Text = "Посмотреть все отзывы";
            this.AllReviewsButton.UseVisualStyleBackColor = true;
            this.AllReviewsButton.Click += new System.EventHandler(this.AllReviewsButton_Click);
            // 
            // AddReviewButton
            // 
            this.AddReviewButton.Location = new System.Drawing.Point(6, 838);
            this.AddReviewButton.Name = "AddReviewButton";
            this.AddReviewButton.Size = new System.Drawing.Size(205, 54);
            this.AddReviewButton.TabIndex = 36;
            this.AddReviewButton.Text = "Оставить отзыв";
            this.AddReviewButton.UseVisualStyleBackColor = true;
            this.AddReviewButton.Click += new System.EventHandler(this.AddReviewButton_Click);
            // 
            // GPReview4Text
            // 
            this.GPReview4Text.Location = new System.Drawing.Point(503, 666);
            this.GPReview4Text.Multiline = true;
            this.GPReview4Text.Name = "GPReview4Text";
            this.GPReview4Text.ReadOnly = true;
            this.GPReview4Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GPReview4Text.Size = new System.Drawing.Size(449, 166);
            this.GPReview4Text.TabIndex = 35;
            // 
            // GPReview4Date
            // 
            this.GPReview4Date.AutoSize = true;
            this.GPReview4Date.Location = new System.Drawing.Point(856, 643);
            this.GPReview4Date.Name = "GPReview4Date";
            this.GPReview4Date.Size = new System.Drawing.Size(100, 20);
            this.GPReview4Date.TabIndex = 34;
            this.GPReview4Date.Text = "Rewiew4Date";
            // 
            // GPReview4User
            // 
            this.GPReview4User.AutoSize = true;
            this.GPReview4User.Location = new System.Drawing.Point(501, 642);
            this.GPReview4User.Name = "GPReview4User";
            this.GPReview4User.Size = new System.Drawing.Size(97, 20);
            this.GPReview4User.TabIndex = 33;
            this.GPReview4User.Text = "Rewiew4User";
            // 
            // GPReview3Text
            // 
            this.GPReview3Text.Location = new System.Drawing.Point(6, 666);
            this.GPReview3Text.Multiline = true;
            this.GPReview3Text.Name = "GPReview3Text";
            this.GPReview3Text.ReadOnly = true;
            this.GPReview3Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GPReview3Text.Size = new System.Drawing.Size(449, 166);
            this.GPReview3Text.TabIndex = 32;
            // 
            // GPReview3Date
            // 
            this.GPReview3Date.AutoSize = true;
            this.GPReview3Date.Location = new System.Drawing.Point(359, 643);
            this.GPReview3Date.Name = "GPReview3Date";
            this.GPReview3Date.Size = new System.Drawing.Size(100, 20);
            this.GPReview3Date.TabIndex = 31;
            this.GPReview3Date.Text = "Rewiew3Date";
            // 
            // GPReview3User
            // 
            this.GPReview3User.AutoSize = true;
            this.GPReview3User.Location = new System.Drawing.Point(4, 642);
            this.GPReview3User.Name = "GPReview3User";
            this.GPReview3User.Size = new System.Drawing.Size(97, 20);
            this.GPReview3User.TabIndex = 30;
            this.GPReview3User.Text = "Rewiew3User";
            // 
            // GPReview2Text
            // 
            this.GPReview2Text.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GPReview2Text.Location = new System.Drawing.Point(503, 451);
            this.GPReview2Text.Multiline = true;
            this.GPReview2Text.Name = "GPReview2Text";
            this.GPReview2Text.ReadOnly = true;
            this.GPReview2Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GPReview2Text.Size = new System.Drawing.Size(449, 179);
            this.GPReview2Text.TabIndex = 29;
            // 
            // GPReview2Date
            // 
            this.GPReview2Date.AutoSize = true;
            this.GPReview2Date.Location = new System.Drawing.Point(856, 428);
            this.GPReview2Date.Name = "GPReview2Date";
            this.GPReview2Date.Size = new System.Drawing.Size(100, 20);
            this.GPReview2Date.TabIndex = 28;
            this.GPReview2Date.Text = "Rewiew2Date";
            // 
            // GPReview2User
            // 
            this.GPReview2User.AutoSize = true;
            this.GPReview2User.Location = new System.Drawing.Point(503, 427);
            this.GPReview2User.Name = "GPReview2User";
            this.GPReview2User.Size = new System.Drawing.Size(97, 20);
            this.GPReview2User.TabIndex = 27;
            this.GPReview2User.Text = "Rewiew2User";
            // 
            // GPReview1Text
            // 
            this.GPReview1Text.Location = new System.Drawing.Point(8, 451);
            this.GPReview1Text.Multiline = true;
            this.GPReview1Text.Name = "GPReview1Text";
            this.GPReview1Text.ReadOnly = true;
            this.GPReview1Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GPReview1Text.Size = new System.Drawing.Size(449, 179);
            this.GPReview1Text.TabIndex = 26;
            // 
            // GPReview1Date
            // 
            this.GPReview1Date.AutoSize = true;
            this.GPReview1Date.Location = new System.Drawing.Point(361, 428);
            this.GPReview1Date.Name = "GPReview1Date";
            this.GPReview1Date.Size = new System.Drawing.Size(96, 20);
            this.GPReview1Date.TabIndex = 25;
            this.GPReview1Date.Text = "Review1Date";
            // 
            // GPReview1User
            // 
            this.GPReview1User.AutoSize = true;
            this.GPReview1User.Location = new System.Drawing.Point(6, 427);
            this.GPReview1User.Name = "GPReview1User";
            this.GPReview1User.Size = new System.Drawing.Size(97, 20);
            this.GPReview1User.TabIndex = 24;
            this.GPReview1User.Text = "Rewiew1User";
            // 
            // GamePageFullCount
            // 
            this.GamePageFullCount.AutoSize = true;
            this.GamePageFullCount.Location = new System.Drawing.Point(762, 376);
            this.GamePageFullCount.Name = "GamePageFullCount";
            this.GamePageFullCount.Size = new System.Drawing.Size(58, 20);
            this.GamePageFullCount.TabIndex = 23;
            this.GamePageFullCount.Text = "label24";
            // 
            // GamePageFullMax
            // 
            this.GamePageFullMax.AutoSize = true;
            this.GamePageFullMax.Location = new System.Drawing.Point(429, 376);
            this.GamePageFullMax.Name = "GamePageFullMax";
            this.GamePageFullMax.Size = new System.Drawing.Size(58, 20);
            this.GamePageFullMax.TabIndex = 22;
            this.GamePageFullMax.Text = "label23";
            // 
            // GamePageFullMid
            // 
            this.GamePageFullMid.AutoSize = true;
            this.GamePageFullMid.Location = new System.Drawing.Point(300, 376);
            this.GamePageFullMid.Name = "GamePageFullMid";
            this.GamePageFullMid.Size = new System.Drawing.Size(58, 20);
            this.GamePageFullMid.TabIndex = 21;
            this.GamePageFullMid.Text = "label21";
            // 
            // GamePageFullMin
            // 
            this.GamePageFullMin.AutoSize = true;
            this.GamePageFullMin.Location = new System.Drawing.Point(592, 376);
            this.GamePageFullMin.Name = "GamePageFullMin";
            this.GamePageFullMin.Size = new System.Drawing.Size(58, 20);
            this.GamePageFullMin.TabIndex = 21;
            this.GamePageFullMin.Text = "label21";
            // 
            // GamePageFullAvg
            // 
            this.GamePageFullAvg.AutoSize = true;
            this.GamePageFullAvg.Location = new System.Drawing.Point(178, 376);
            this.GamePageFullAvg.Name = "GamePageFullAvg";
            this.GamePageFullAvg.Size = new System.Drawing.Size(58, 20);
            this.GamePageFullAvg.TabIndex = 20;
            this.GamePageFullAvg.Text = "label20";
            // 
            // GamePageNormalCount
            // 
            this.GamePageNormalCount.AutoSize = true;
            this.GamePageNormalCount.Location = new System.Drawing.Point(762, 330);
            this.GamePageNormalCount.Name = "GamePageNormalCount";
            this.GamePageNormalCount.Size = new System.Drawing.Size(58, 20);
            this.GamePageNormalCount.TabIndex = 19;
            this.GamePageNormalCount.Text = "label19";
            // 
            // GamePageNormalMin
            // 
            this.GamePageNormalMin.AutoSize = true;
            this.GamePageNormalMin.Location = new System.Drawing.Point(592, 330);
            this.GamePageNormalMin.Name = "GamePageNormalMin";
            this.GamePageNormalMin.Size = new System.Drawing.Size(58, 20);
            this.GamePageNormalMin.TabIndex = 18;
            this.GamePageNormalMin.Text = "label18";
            // 
            // GamePageNormalMax
            // 
            this.GamePageNormalMax.AutoSize = true;
            this.GamePageNormalMax.Location = new System.Drawing.Point(429, 330);
            this.GamePageNormalMax.Name = "GamePageNormalMax";
            this.GamePageNormalMax.Size = new System.Drawing.Size(58, 20);
            this.GamePageNormalMax.TabIndex = 17;
            this.GamePageNormalMax.Text = "label17";
            // 
            // GamePageNormalMid
            // 
            this.GamePageNormalMid.AutoSize = true;
            this.GamePageNormalMid.Location = new System.Drawing.Point(300, 330);
            this.GamePageNormalMid.Name = "GamePageNormalMid";
            this.GamePageNormalMid.Size = new System.Drawing.Size(58, 20);
            this.GamePageNormalMid.TabIndex = 16;
            this.GamePageNormalMid.Text = "label16";
            // 
            // GamePageNormalAvg
            // 
            this.GamePageNormalAvg.AutoSize = true;
            this.GamePageNormalAvg.Location = new System.Drawing.Point(178, 330);
            this.GamePageNormalAvg.Name = "GamePageNormalAvg";
            this.GamePageNormalAvg.Size = new System.Drawing.Size(58, 20);
            this.GamePageNormalAvg.TabIndex = 15;
            this.GamePageNormalAvg.Text = "label15";
            // 
            // GamePageCount
            // 
            this.GamePageCount.AutoSize = true;
            this.GamePageCount.Location = new System.Drawing.Point(710, 286);
            this.GamePageCount.Name = "GamePageCount";
            this.GamePageCount.Size = new System.Drawing.Size(150, 20);
            this.GamePageCount.TabIndex = 14;
            this.GamePageCount.Text = "Количество отметок";
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(6, 16);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(36, 35);
            this.BackButton.TabIndex = 13;
            this.BackButton.Text = "<";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // GamePageScore
            // 
            this.GamePageScore.AutoSize = true;
            this.GamePageScore.Location = new System.Drawing.Point(762, 246);
            this.GamePageScore.Name = "GamePageScore";
            this.GamePageScore.Size = new System.Drawing.Size(117, 20);
            this.GamePageScore.TabIndex = 12;
            this.GamePageScore.Text = "GamePageScore";
            // 
            // GamePageFull
            // 
            this.GamePageFull.AutoSize = true;
            this.GamePageFull.Location = new System.Drawing.Point(6, 376);
            this.GamePageFull.Name = "GamePageFull";
            this.GamePageFull.Size = new System.Drawing.Size(163, 20);
            this.GamePageFull.TabIndex = 11;
            this.GamePageFull.Text = "Полное прохождение";
            // 
            // GamePageMin
            // 
            this.GamePageMin.AutoSize = true;
            this.GamePageMin.Location = new System.Drawing.Point(546, 286);
            this.GamePageMin.Name = "GamePageMin";
            this.GamePageMin.Size = new System.Drawing.Size(158, 20);
            this.GamePageMin.TabIndex = 10;
            this.GamePageMin.Text = "Минимальное время";
            // 
            // GamePageMax
            // 
            this.GamePageMax.AutoSize = true;
            this.GamePageMax.Location = new System.Drawing.Point(378, 286);
            this.GamePageMax.Name = "GamePageMax";
            this.GamePageMax.Size = new System.Drawing.Size(162, 20);
            this.GamePageMax.TabIndex = 9;
            this.GamePageMax.Text = "Максимальное время";
            // 
            // GamePageMid
            // 
            this.GamePageMid.AutoSize = true;
            this.GamePageMid.Location = new System.Drawing.Point(286, 286);
            this.GamePageMid.Name = "GamePageMid";
            this.GamePageMid.Size = new System.Drawing.Size(72, 20);
            this.GamePageMid.TabIndex = 8;
            this.GamePageMid.Text = "Медиана";
            // 
            // GamePageAvg
            // 
            this.GamePageAvg.AutoSize = true;
            this.GamePageAvg.Location = new System.Drawing.Point(153, 286);
            this.GamePageAvg.Name = "GamePageAvg";
            this.GamePageAvg.Size = new System.Drawing.Size(116, 20);
            this.GamePageAvg.TabIndex = 7;
            this.GamePageAvg.Text = "Среднее время";
            // 
            // GamePageNormal
            // 
            this.GamePageNormal.AutoSize = true;
            this.GamePageNormal.Location = new System.Drawing.Point(6, 330);
            this.GamePageNormal.Name = "GamePageNormal";
            this.GamePageNormal.Size = new System.Drawing.Size(128, 20);
            this.GamePageNormal.TabIndex = 6;
            this.GamePageNormal.Text = "Основной сюжет";
            // 
            // GamePageTimeStat
            // 
            this.GamePageTimeStat.AutoSize = true;
            this.GamePageTimeStat.Location = new System.Drawing.Point(6, 246);
            this.GamePageTimeStat.Name = "GamePageTimeStat";
            this.GamePageTimeStat.Size = new System.Drawing.Size(263, 20);
            this.GamePageTimeStat.TabIndex = 5;
            this.GamePageTimeStat.Text = "Статистика о времени прохождения";
            // 
            // GamePagePlatform
            // 
            this.GamePagePlatform.AutoSize = true;
            this.GamePagePlatform.Location = new System.Drawing.Point(153, 204);
            this.GamePagePlatform.Name = "GamePagePlatform";
            this.GamePagePlatform.Size = new System.Drawing.Size(137, 20);
            this.GamePagePlatform.TabIndex = 4;
            this.GamePagePlatform.Text = "GamePagePlatform";
            // 
            // GamePagePublisher
            // 
            this.GamePagePublisher.AutoSize = true;
            this.GamePagePublisher.Location = new System.Drawing.Point(153, 159);
            this.GamePagePublisher.Name = "GamePagePublisher";
            this.GamePagePublisher.Size = new System.Drawing.Size(140, 20);
            this.GamePagePublisher.TabIndex = 3;
            this.GamePagePublisher.Text = "GamePagePublisher";
            // 
            // GamePageDeveloper
            // 
            this.GamePageDeveloper.AutoSize = true;
            this.GamePageDeveloper.Location = new System.Drawing.Point(153, 114);
            this.GamePageDeveloper.Name = "GamePageDeveloper";
            this.GamePageDeveloper.Size = new System.Drawing.Size(149, 20);
            this.GamePageDeveloper.TabIndex = 2;
            this.GamePageDeveloper.Text = "GamePageDeveloper";
            // 
            // GamePageReleaseDate
            // 
            this.GamePageReleaseDate.AutoSize = true;
            this.GamePageReleaseDate.Location = new System.Drawing.Point(153, 70);
            this.GamePageReleaseDate.Name = "GamePageReleaseDate";
            this.GamePageReleaseDate.Size = new System.Drawing.Size(163, 20);
            this.GamePageReleaseDate.TabIndex = 1;
            this.GamePageReleaseDate.Text = "GamePageReleaseDate";
            // 
            // GamePageTitle
            // 
            this.GamePageTitle.AutoSize = true;
            this.GamePageTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.GamePageTitle.Location = new System.Drawing.Point(48, 23);
            this.GamePageTitle.Name = "GamePageTitle";
            this.GamePageTitle.Size = new System.Drawing.Size(115, 20);
            this.GamePageTitle.TabIndex = 0;
            this.GamePageTitle.Text = "GamePageTitle";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(774, 10);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(105, 27);
            this.RegisterButton.TabIndex = 1;
            this.RegisterButton.Text = "Регистрация";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // AuthButton
            // 
            this.AuthButton.Location = new System.Drawing.Point(698, 10);
            this.AuthButton.Name = "AuthButton";
            this.AuthButton.Size = new System.Drawing.Size(70, 27);
            this.AuthButton.TabIndex = 2;
            this.AuthButton.Text = "Вход";
            this.AuthButton.UseVisualStyleBackColor = true;
            this.AuthButton.Click += new System.EventHandler(this.AuthButton_Click);
            // 
            // LogoutButton
            // 
            this.LogoutButton.Location = new System.Drawing.Point(622, 10);
            this.LogoutButton.Name = "LogoutButton";
            this.LogoutButton.Size = new System.Drawing.Size(70, 27);
            this.LogoutButton.TabIndex = 3;
            this.LogoutButton.Text = "Выход";
            this.LogoutButton.UseVisualStyleBackColor = true;
            this.LogoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Location = new System.Drawing.Point(360, 10);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.PlaceholderText = "Логин";
            this.LoginTextBox.Size = new System.Drawing.Size(125, 27);
            this.LoginTextBox.TabIndex = 4;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(491, 10);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PlaceholderText = "Пароль";
            this.PasswordTextBox.Size = new System.Drawing.Size(125, 27);
            this.PasswordTextBox.TabIndex = 5;
            // 
            // LoginMessage
            // 
            this.LoginMessage.AutoSize = true;
            this.LoginMessage.ForeColor = System.Drawing.Color.Black;
            this.LoginMessage.Location = new System.Drawing.Point(12, 10);
            this.LoginMessage.Name = "LoginMessage";
            this.LoginMessage.Size = new System.Drawing.Size(0, 20);
            this.LoginMessage.TabIndex = 6;
            // 
            // AddGameButton
            // 
            this.AddGameButton.Location = new System.Drawing.Point(885, 0);
            this.AddGameButton.Name = "AddGameButton";
            this.AddGameButton.Size = new System.Drawing.Size(102, 56);
            this.AddGameButton.TabIndex = 7;
            this.AddGameButton.Text = "Добавить игру";
            this.AddGameButton.UseVisualStyleBackColor = true;
            this.AddGameButton.Click += new System.EventHandler(this.AddGameButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 47;
            this.label1.Text = "Дата выхода";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 20);
            this.label2.TabIndex = 48;
            this.label2.Text = "Платформы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.TabIndex = 49;
            this.label3.Text = "Разработчик";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 50;
            this.label4.Text = "Издатель";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(447, 407);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 51;
            this.label5.Text = "Отзывы";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 953);
            this.Controls.Add(this.AddGameButton);
            this.Controls.Add(this.LoginMessage);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.LoginTextBox);
            this.Controls.Add(this.LogoutButton);
            this.Controls.Add(this.AuthButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.GamePage);
            this.Controls.Add(this.LibraryGroupbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "GameTime";
            this.LibraryGroupbox.ResumeLayout(false);
            this.GamePage.ResumeLayout(false);
            this.GamePage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private List<Button> libraryButtons;
        private GroupBox LibraryGroupbox;
        private GroupBox GamePage;
        private Label GamePageCount;
        private Button BackButton;
        private Label GamePageScore;
        private Label GamePageFull;
        private Label GamePageMin;
        private Label GamePageMax;
        private Label GamePageMid;
        private Label GamePageAvg;
        private Label GamePageNormal;
        private Label GamePageTimeStat;
        private Label GamePagePlatform;
        private Label GamePagePublisher;
        private Label GamePageDeveloper;
        private Label GamePageReleaseDate;
        private Label GamePageTitle;
        private Label GamePageFullCount;
        private Label GamePageFullMax;
        private Label GamePageFullMid;
        private Label GamePageFullMin;
        private Label GamePageFullAvg;
        private Label GamePageNormalCount;
        private Label GamePageNormalMin;
        private Label GamePageNormalMax;
        private Label GamePageNormalMid;
        private Label GamePageNormalAvg;
        private Button RegisterButton;
        private Button AuthButton;
        private Button LogoutButton;
        private TextBox LoginTextBox;
        private TextBox PasswordTextBox;
        private Label LoginMessage;
        private Button AllReviewsButton;
        private Button AddReviewButton;
        private TextBox GPReview4Text;
        private Label GPReview4Date;
        private Label GPReview4User;
        private TextBox GPReview3Text;
        private Label GPReview3Date;
        private Label GPReview3User;
        private TextBox GPReview2Text;
        private Label GPReview2Date;
        private Label GPReview2User;
        private TextBox GPReview1Text;
        private Label GPReview1Date;
        private Label GPReview1User;
        private Button UpdateGameButton;
        private Button DeleteGameButton;
        private VScrollBar vScrollBar1;
        private Label GPReview4Score;
        private Label GPReview3Score;
        private Label GPReview2Score;
        private Label GPReview1Score;
        private Button AllTimeRecords;
        private Button AddTimeRecordButton;
        private Button AddGameButton;
        private Label Score;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
    }
}