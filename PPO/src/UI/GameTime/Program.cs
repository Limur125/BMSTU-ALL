using Microsoft.Extensions.DependencyInjection;
using BusinesLogic;
using SqlServerDatabase;
namespace GameTime
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ITimeRecordService, TimeRecordService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGameRepository, SqlServerGameRepository>();
            services.AddTransient<IReviewRepository, SqlServerReviewRepository>();
            services.AddTransient<ITimeRecordRepository, SqlServerTimeRecordRepository>();
            services.AddTransient<IUserRepository, SqlServerUserRepository>();
            //services.AddSingleton<ILogger, FileLogger>();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(services));
        }
    }
}