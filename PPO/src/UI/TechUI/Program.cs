using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace TechUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ITimeRecordService, TimeRecordService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGameRepository, SqlServerGameRepository>();
            services.AddTransient<IReviewRepository, SqlServerReviewRepository>();
            services.AddTransient<ITimeRecordRepository, SqlServerTimeRecordRepository>();
            services.AddTransient<IUserRepository, SqlServerUserRepository>();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(services));
        }
    }
}