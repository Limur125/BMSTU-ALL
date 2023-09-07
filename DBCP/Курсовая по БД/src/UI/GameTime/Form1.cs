using BusinesLogic;
using Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using SqlServerDatabase;

namespace GameTime
{
    public partial class Form1 : Form
    {
        private IGameService gameService;
        private IReviewService reviewService;
        private ITimeRecordService timeRecordService;
        private IUserService userService;
        private ILogger logger;
        private GameInfo currentGame;
        private string userLogin;
        private IConfiguration AppConfiguration;
        private UserMode mode;
        public Form1(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider();
            gameService = scope.GetRequiredService<IGameService>();
            reviewService = scope.GetRequiredService<IReviewService>();
            timeRecordService = scope.GetRequiredService<ITimeRecordService>();
            userService = scope.GetRequiredService<IUserService>();
            GetLogger();
            logger.LogInformation("{} Start Application", DateTime.Now);
            InitializeComponent();
            AddLibraryButtons();
            GamePage.Visible = false;
            EnterGuestMode();
        }
        
        List<string> GetLibrary()
        {
            logger.LogTrace("{} Get Library", DateTime.Now);
            return gameService.GetLibrary();
        }

        void DisableReviews()
        {
            for (int i = 1; i <= 4; i++)
            {
                GamePage.Controls[$"GPReview{i}Date"].Visible = false;
                GamePage.Controls[$"GPReview{i}Text"].Visible = false;
                GamePage.Controls[$"GPReview{i}User"].Visible = false;
                GamePage.Controls[$"GPReview{i}Score"].Visible = false;
            }
        }

        void EnableReview(int n)
        {
            GameInfo gi = currentGame;
            GamePage.Controls[$"GPReview{n}Date"].Visible = true;
            GamePage.Controls[$"GPReview{n}Text"].Visible = true;
            GamePage.Controls[$"GPReview{n}User"].Visible = true;
            GamePage.Controls[$"GPReview{n}Score"].Visible = true;
            GamePage.Controls[$"GPReview{n}User"].Text = gi.Reviews[n - 1].UserLogin;
            GamePage.Controls[$"GPReview{n}Date"].Text = gi.Reviews[n - 1].PublicationDate.Date.ToShortDateString();
            GamePage.Controls[$"GPReview{n}Text"].Text = gi.Reviews[n - 1].Text;
            GamePage.Controls[$"GPReview{n}Score"].Text = gi.Reviews[n - 1].Score.ToString();
        }
       
        private void SelectGame(object sender, EventArgs e)
        {
            logger.LogTrace("{} Get Game", DateTime.Now);
            Button button = sender as Button;
            string title = button.Text;
            GameInfo gi = gameService.GetGame(title);
            logger.LogTrace("{} Game \"{}\" recieved", DateTime.Now, gi.Game.Title);
            currentGame = gi;
            LibraryGroupbox.Visible = false;
            DisableReviews();
            GamePageTitle.Text = title;
            GamePageReleaseDate.Text = gi.Game.ReleaseDate.ToShortDateString();
            GamePageDeveloper.Text = gi.Game.Developer;
            GamePagePublisher.Text = gi.Game.Publisher;
            GamePagePlatform.Text = gi.Game.Genres;
            GamePageScore.Text = gi.AvgScore.ToString();
            GamePageNormalAvg.Text = gi.NormalTime.AvgTime.ToString();
            GamePageNormalMid.Text = gi.NormalTime.MinTime.ToString();
            GamePageNormalMax.Text = gi.NormalTime.MaxTime.ToString();
            GamePageNormalMin.Text = gi.NormalTime.MinTime.ToString();
            GamePageNormalCount.Text = gi.NormalTime.Count.ToString();
            GamePageFullAvg.Text = gi.FullTime.AvgTime.ToString();
            GamePageFullMid.Text = gi.FullTime.MinTime.ToString();
            GamePageFullMax.Text = gi.FullTime.MaxTime.ToString();
            GamePageFullMin.Text = gi.FullTime.MinTime.ToString();
            GamePageFullCount.Text = gi.FullTime.Count.ToString();
            GamePage.Visible = true;
            GamePage.Text = title;
            for (int i = 1; i <= gi.Reviews.Count; i++)
                EnableReview(i);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            logger.LogTrace("{} Return to library", DateTime.Now);
            GamePage.Visible = false;
            LibraryGroupbox.Visible = true;
            currentGame = null;
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            logger.LogTrace("{} User {} Logout", DateTime.Now);
            LoginError.Text = "";
            EnterGuestMode();
            userLogin = "";
        }

        private void AuthButton_Click(object sender, EventArgs e)
        {
            LoginError.Text = "";
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            if (login.Length < 3)
            {
                LoginError.Text = "Логин должен содержать не менее 3 символов";
                return;
            }
            if (password.Length < 3)
            {
                LoginError.Text = "Пароль должен содержать не менее 3 символов";
                return;
            }
            if (login == "admin" && password == "admin")
            {
                EnterAdminMode();
                userLogin = login;
                return;
            }
            User user = new User(login, password);
            try
            {
                userService.LogIn(user);
            }
            catch
            {
                LoginError.Text = "Такого пользователя не существует";
                return;
            }
            EnterUserMode();
            userLogin = login;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            LoginError.Text = "";
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            if (login.Length < 3)
            {
                LoginError.Text = "Логин должен содержать не менее 3 символов";
                return;
            }
            if (password.Length < 3)
            {
                LoginError.Text = "Пароль должен содержать не менее 3 символов";
                return;
            }
            if (login == "admin" && password == "admin")
            {
                userLogin = login;
                EnterAdminMode();
                return;
            }
            User user = new User(login, password);
            try
            {
                userService.Register(user);
            }
            catch
            {
                LoginError.Text = "Такой пользователь уже существует";
                return;
            }
            EnterUserMode();
            userLogin = login;
        }
        void EnterAdminMode()
        {
            mode = UserMode.ADMIN;
            LogoutButton.Enabled = true;
            AuthButton.Enabled = false;
            RegisterButton.Enabled = false;
            DeleteGameButton.Visible = true;
            UpdateGameButton.Visible = true;
            AddReviewButton.Visible = true;
            AddTimeRecordButton.Visible = true;
            AddGameButton.Visible = true;
        }
        void EnterUserMode()
        {
            mode = UserMode.USER;
            LogoutButton.Enabled = true;
            AuthButton.Enabled = false;
            RegisterButton.Enabled = false;
            DeleteGameButton.Visible = false;
            UpdateGameButton.Visible = false;
            AddReviewButton.Visible = true;
            AddTimeRecordButton.Visible = true;
            AddGameButton.Visible = false;
        }
        void EnterGuestMode()
        {
            mode = UserMode.GUEST;
            LogoutButton.Enabled = false;
            AuthButton.Enabled = true;
            RegisterButton.Enabled = true;
            DeleteGameButton.Visible = false;
            UpdateGameButton.Visible = false;
            AddReviewButton.Visible = false;
            AddTimeRecordButton.Visible = false;
            AddGameButton.Visible = false;
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (var button in libraryButtons)
            {
                int x = button.Location.X;
                int y = button.Location.Y;
                button.Location = new Point(x, y - 78 * (e.NewValue - e.OldValue));
                if (y > 0 && y < LibraryGroupbox.Size.Height)
                    button.Visible = true;
                else
                    button.Visible = false;
            }
        }
        void GetLogger()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            AppConfiguration = builder.Build();
            string logLevelStr = AppConfiguration["LogLevel"];
            LogLevel logLevel = logLevelStr switch
            {
                "Trace" => LogLevel.Trace,
                "Debug" => LogLevel.Debug,
                "Information" => LogLevel.Information,
                "Warning" => LogLevel.Warning,
                "Error" => LogLevel.Error,
                "Critical" => LogLevel.Critical,
                _ => LogLevel.None,
            };
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddFilter("Default", logLevel));
            logger = loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("Default");
        }

        private void DeleteGameButton_Click(object sender, EventArgs e)
        {
            gameService.DeleteGame(currentGame.Game.Title);
            RefreshLibraryButtons();
            BackButton_Click(sender, e);
        }

        private void UpdateGameButton_Click(object sender, EventArgs e)
        {
            new AddGameForm(this, GameMode.UPDATE, currentGame.Game).ShowDialog();
            Button button = new Button();
            button.Text = currentGame.Game.Title;
            SelectGame(button, e);
            RefreshLibraryButtons();
        }

        private void AddGameButton_Click(object sender, EventArgs e)
        {
            new AddGameForm(this, GameMode.ADD, null).ShowDialog();
            RefreshLibraryButtons();
        }

        public void AddGame(Game g)
        {
            try
            {
                gameService.AddGame(g);
            }
            catch (Exception e)
            {
                logger.LogError("{} Add game failed \n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
            }
        }

        public void UpdateGame(Game g)
        {
            try 
            { 
                gameService.UpdateGame(g);
                currentGame = new GameInfo(g, 0, null, null, null);
            }
            catch (Exception e)
            {
                logger.LogError("{} Update game failed \n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
            }
        }

        public void DeleteReview(Review r)
        {
            try
            {
                reviewService.DeleteReview(r);
            }
            catch (Exception e)
            {
                logger.LogError("{} Delete review failed \n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
            }
        }

        public void DeleteRecord(TimeRecord r)
        {
            try
            {
                timeRecordService.DeleteTimeRecord(r);
            }
            catch (Exception e)
            {
                logger.LogError("{} Delete time record failed \n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
            }
        }
        public void SaveReview(string text, uint score)
        {
            try
            {
                Review r = new Review(currentGame.Game.Id, userLogin, score, text, DateTime.Today);
                reviewService.SaveReview(r);
            }
            catch (Exception e)
            {
                logger.LogError("{} Save review failed \n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
            }
        }

        public void SaveTimeRecord(int type, string hoursStr, string minutesStr)
        {
            TimeRecordType trt;
            try
            {
                trt = type switch
                {
                    0 => TimeRecordType.NORMAL,
                    1 => TimeRecordType.FULL,
                    _ => throw new FormatException("Invalid type"),
                };
            }
            catch (Exception e)
            {
                logger.LogError("{} SaveTimeRecord convert minutes and hours failed\n{}\n{}", DateTime.Now, e.Message, e.StackTrace);
                return;
            }
            int hours, minutes;
            try
            {
                hours = Convert.ToInt32(hoursStr);
                minutes = Convert.ToInt32(minutesStr);
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveTimeRecord convert minutes and hours failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                return;
            }
            TimeSpan time = new TimeSpan(hours, minutes, 0);
            TimeRecord record = new TimeRecord(currentGame.Game.Id, time, trt, userLogin);
            try
            {
                timeRecordService.AddTimeRecord(record);
                logger.LogTrace("{} SaveTimeRecord succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
            }
        }
        private void AddReviewButton_Click(object sender, EventArgs e)
        {
            //List<Review> reviews = reviewService.GetReviews(currentGame.Game.Id);
            new ReviewForm(this, null, ReviewMode.ADD, mode).ShowDialog();
        }

        private void AllReviewsButton_Click(object sender, EventArgs e)
        {
            List<Review> reviews = reviewService.GetReviews(currentGame.Game.Id);
            new ReviewForm(this, reviews, ReviewMode.SHOW, mode).ShowDialog();
        }

        private void AddTimeRecordButton_Click(object sender, EventArgs e)
        {
            new TimeRecordForm(this, TimeMode.ADD, null, mode).ShowDialog();
        }

        private void AllTimeRecords_Click(object sender, EventArgs e)
        {
            List<TimeRecord> records = timeRecordService.GetTimeRecords(currentGame.Game.Id);
            new TimeRecordForm(this, TimeMode.ALL, records, mode).ShowDialog();
        }
    }

    public enum UserMode
    {
        ADMIN,
        USER,
        GUEST,
    }
}