using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace TechUI
{
    public partial class Form1 : Form
    {
        private IGameService gameService;
        private IReviewService reviewService;
        private ITimeRecordService timeRecordService;
        private IUserService userService;
        private ILogger logger;
        IConfiguration AppConfiguration;
        private string? userName = null;
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
            GuestMode();
        }

        private void authButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} Authorization button pressed", DateTime.Now);
            string login = authLoginTextbox.Text;
            string password = AuthPasswordTextbox.Text;
            if (login == "admin" && password == "admin")
            {
                logger.LogInformation("{} Authorization succeeded", DateTime.Now);
                AdminMode();
                userName = login;
                return;
            }
            User u = new User(login, password);
            try
            {
                userService.LogIn(u);
                userName = login;
                logger.LogInformation("{} Authorization succeeded", DateTime.Now);
                UserMode();
            }
            catch (Exception ex)
            {
                logger.LogError("{} Authorization failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} Logout button pressed", DateTime.Now);
            userName = null;
            GuestMode();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} Registration button pressed", DateTime.Now);
            string login = registerLoginTextbox.Text;
            string password = registerPasswordTextbox.Text;
            User u = new User(login, password);
            try
            {
                userService.Register(u);
                userName = login;
                logger.LogInformation("{} Registration succeeded", DateTime.Now);
                UserMode();
            }
            catch (Exception ex)
            {
                logger.LogError("{} Registration failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void LibraryButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} GetLibrary button pressed", DateTime.Now);
            try
            {
                List<string> games = gameService.GetLibrary();
                logger.LogInformation("{} GetLibrary succeeded", DateTime.Now);
                if (!games.Any())
                {
                    Console.WriteLine("No Games");
                }
                else
                {
                    Console.WriteLine("\nLibrary\n");
                    foreach (string game in games)
                        Console.WriteLine(game);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{} GetLibrary failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void GetGameButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} GetGame button pressed", DateTime.Now);
            string title = GetGameTitleTextbox.Text;
            try
            {
                GameInfo gi = gameService.GetGame(title);
                logger.LogInformation("{} GetGame succeeded", DateTime.Now);
                Console.WriteLine($"{gi.Game.Title}");
                Console.WriteLine($"Дата выхода {gi.Game.ReleaseDate.ToShortDateString()}");
                Console.WriteLine($"Разработчик {gi.Game.Developer}");
                Console.WriteLine($"Издатель {gi.Game.Publisher}");
                Console.WriteLine($"Платформа {gi.Game.Genres}\n");
                Console.WriteLine($"Среднее время нормального прохождение {gi.NormalTime.AvgTime} пользователей {gi.NormalTime.Count}");
                Console.WriteLine($"Медианное время нормального прохождение {gi.NormalTime.MidTime}");
                Console.WriteLine($"Максимальное время нормального прохождение {gi.NormalTime.MaxTime}");
                Console.WriteLine($"Минимальное время нормального прохождение {gi.NormalTime.MinTime}\n");
                Console.WriteLine($"Среднее время полного прохождение {gi.FullTime.AvgTime} пользователей {gi.FullTime.Count}");
                Console.WriteLine($"Медианное время полного прохождение {gi.FullTime.MidTime}");
                Console.WriteLine($"Максимальное время полного прохождение {gi.FullTime.MaxTime}");
                Console.WriteLine($"Минимальное время полного прохождение {gi.FullTime.MinTime}");
                Console.WriteLine($"Средняя оценка игры {gi.AvgScore}");
                Console.WriteLine("Отзывы\n");
                foreach (Review r in gi.Reviews)
                {
                    Console.WriteLine($"Пользователь {r.UserLogin} оставил отзыв {r.PublicationDate.ToShortDateString()} с оценкой {r.Score}");
                    Console.WriteLine($"{r.Text}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{} GetGame failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }

        }

        private void SaveReviewButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} SaveReview button pressed", DateTime.Now);
            if (userName is null)
                return;
            string title = SaveReviewTitleTextbox.Text;
            DateTime publicationDate = DateTime.Today;
            string text = SaveReviewTextTextbox.Text;
            int id;
            try
            {
                id = gameService.GetGame(title).Game.Id;
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveReview failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            uint score = Convert.ToUInt32(SaveReviewScoreNumericUpDown.Value);
            Review review = new Review(id, userName, score, text, publicationDate);
            try
            {
                reviewService.SaveReview(review);
                logger.LogInformation("{} SaveReview succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveReview failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void SaveTimeRecordButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} SaveTimeRecord button pressed", DateTime.Now);
            if (userName is null)
                return;
            string title = SaveTimeRecordTitleTextbox.Text;
            int type = SaveTimeRecordTypeCombobox.SelectedIndex;
            TimeRecordType trt = type switch
            {
                0 => TimeRecordType.NORMAL,
                1 => TimeRecordType.FULL,
                _ => TimeRecordType.NORMAL,
            };
            int hours, minutes;
            try
            {
                hours = Convert.ToInt32(SaveTimeRecordHoursTextbox.Text);
                minutes = Convert.ToInt32(SaveTimeRecordMinutesTextbox.Text);
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            int id;
            try
            {
                id = gameService.GetGame(title).Game.Id;
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            TimeSpan time = new TimeSpan(hours, minutes, 0);
            TimeRecord record = new TimeRecord(id, time, trt, userName);
            try
            {
                timeRecordService.AddTimeRecord(record);
                logger.LogInformation("{} SaveTimeRecord succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} SaveTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void AddGameButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} AddGame button pressed", DateTime.Now);
            string title = AddGameTitleTextbox.Text;
            DateTime releaseDate = AddGameReleaseDateCalendar.SelectionStart;
            string developer = AddGameDeveloperTextbox.Text;
            string publisher = AddGamePublisherTextbox.Text;
            string platform = AddGameGenresTextbox.Text;
            Game g = new Game(0, title, releaseDate, developer, publisher, platform);
            try
            {
                gameService.AddGameXMLSQL(g);
                logger.LogInformation("{} AddGame succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} AddGame failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteGameButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} DeleteGame button pressed", DateTime.Now);
            string title = DeleteGameTitleTextbox.Text;
            try
            {
                gameService.DeleteGame(title);
                logger.LogInformation("{} DeleteGame succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} DeleteGame failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteReviewButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} DeleteReview button pressed", DateTime.Now);
            string title = DeleteReviewTitleTextbox.Text;
            string user = DeleteReviewUserTextbox.Text;
            int id;
            try
            {
                id = gameService.GetGame(title).Game.Id;
            }
            catch (Exception ex)
            {
                logger.LogError("{} DeleteReview failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            Review review = new Review(id, user, 0, "", DateTime.Now);
            try
            {
                reviewService.DeleteReview(review);
                logger.LogInformation("{} DeleteReview succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} DeleteReview failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteTimeRecordButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} DeleteTimeRecord button pressed", DateTime.Now);
            string title = DeleteTimeRecordTitleTextbox.Text;
            string user = DeleteTimeRecordUserTextbox.Text;
            int id;
            try
            {
                id = gameService.GetGame(title).Game.Id;
            }
            catch (Exception ex)
            {
                logger.LogError("{} DeleteTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            TimeRecord record = new TimeRecord(id, TimeSpan.Zero, TimeRecordType.NORMAL, user);
            try
            {
                timeRecordService.DeleteTimeRecord(record);
                logger.LogInformation("{} DeleteTimeRecord succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} DeleteTimeRecord failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateGameButton_Click(object sender, EventArgs e)
        {
            logger.LogInformation("{} UpdateGame button pressed", DateTime.Now);
            string title = UpdateGameTitleTextbox.Text;
            DateTime releaseDate = UpdateGameReleaseDateCalendar.SelectionStart;
            string developer = UpdateGameDeveloperTextbox.Text;
            string publisher = UpdateGamePublisherTextbox.Text;
            string platform = UpdateGamePlatformTextbox.Text;
            int id;
            try
            {
                id = gameService.GetGame(title).Game.Id;
            }
            catch (Exception ex)
            {
                logger.LogError("{} UpdateGame failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
                return;
            }
            Game g = new Game(id, title, releaseDate, developer, publisher, platform);
            try
            {
                gameService.UpdateGame(g);
                logger.LogInformation("{} UpdateGame succeeded", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("{} UpdateGame failed\n{}\n{}", DateTime.Now, ex.Message, ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void AdminMode()
        {
            logger.LogInformation("{} AdminMode enter", DateTime.Now);
            authButton.Enabled = false;
            registerButton.Enabled = false;
            logoutButton.Enabled = true;
            GetGameButton.Enabled = true;
            LibraryButton.Enabled = true;
            SaveReviewButton.Enabled = true;
            SaveTimeRecordButton.Enabled = true;
            AddGameButton.Enabled = true;
            DeleteGameButton.Enabled = true;
            DeleteReviewButton.Enabled = true;
            DeleteTimeRecordButton.Enabled = true;
            UpdateGameButton.Enabled = true;
        }

        private void UserMode()
        {
            logger.LogInformation("{} UserMode enter", DateTime.Now);
            authButton.Enabled = false;
            registerButton.Enabled = false;
            logoutButton.Enabled = true;
            GetGameButton.Enabled = true;
            LibraryButton.Enabled = true;
            SaveReviewButton.Enabled = true;
            SaveTimeRecordButton.Enabled = true;
            AddGameButton.Enabled = false;
            DeleteGameButton.Enabled = false;
            DeleteReviewButton.Enabled = false;
            DeleteTimeRecordButton.Enabled = false;
            UpdateGameButton.Enabled = false;
        }

        private void GuestMode()
        {
            logger.LogInformation("{} GuestMode enter", DateTime.Now);
            authButton.Enabled = true;
            registerButton.Enabled = true;
            logoutButton.Enabled = false;
            GetGameButton.Enabled = true;
            LibraryButton.Enabled = true;
            SaveReviewButton.Enabled = false;
            SaveTimeRecordButton.Enabled = false;
            AddGameButton.Enabled = false;
            DeleteGameButton.Enabled = false;
            DeleteReviewButton.Enabled = false;
            DeleteTimeRecordButton.Enabled = false;
            UpdateGameButton.Enabled = false;
        }
        void GetLogger()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            AppConfiguration = builder.Build();
            string logLevelStr = AppConfiguration["LogLevel"];
            LogLevel logLevel = logLevelStr switch
            {
                "Error" => LogLevel.Error,
                "Information" => LogLevel.Information,
                "Debug" => LogLevel.Debug,
                "Critical" => LogLevel.Critical,
                "Warning" => LogLevel.Warning,
                "Trace" => LogLevel.Trace,
                _ => LogLevel.None,
            };
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddFilter("Default", logLevel));
            logger = loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("Default");
        }

        private void CompareButton_Click(object sender, EventArgs e)
        {
            int[] count = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
            int k = 0;
            foreach(int n in count)
            {
                Stopwatch sw = new Stopwatch();
                for (int j = 0; j < 14; j++)
                {
                    sw.Start();
                    for (int i = 0; i < n; i++)
                    {
                        Game g = new Game(0, $"sql{k++}", DateTime.Today, "dev", "pub", "aaa bbb ccc ddd eee fff ggg hhh jjj kkk lll ppp ooo iii uuu yyy ttt rrr www");

                        gameService.AddGameXMLSQL(g);

                    }
                    sw.Stop();
                }
                Console.WriteLine("{0:f1} \t\t\t", sw.ElapsedMilliseconds / 14.0);
                sw.Reset();
                for (int j = 0; j < 14; j++)
                {
                    sw.Start();
                    for (int i = 0; i < n; i++)
                    {
                        Game g = new Game(0, $"app{k++}", DateTime.Today, "dev", "pub", "aaa bbb ccc ddd eee fff ggg hhh jjj kkk lll ppp ooo iii uuu yyy ttt rrr www");
                        gameService.AddGameXMLApp(g);
                    }
                    sw.Stop();
                }
                Console.WriteLine("{0:f1}\n", sw.ElapsedMilliseconds / 14.0);

            }
        }
    }
}