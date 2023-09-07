using BusinesLogic;
using Microsoft.Extensions.Configuration;

namespace SqlServerDatabase.Repositories
{
    public abstract class SqlServerRepository
    {
        protected readonly string guestConnection;
        protected readonly string userConnection;
        protected readonly string adminConnection;
        private IConfiguration AppConfiguration;

        protected SqlServerRepository()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            AppConfiguration = builder.Build();
            guestConnection = AppConfiguration["Connection"];
            userConnection = AppConfiguration["Connection"];
            adminConnection = AppConfiguration["Connection"];
        }
    }
}
