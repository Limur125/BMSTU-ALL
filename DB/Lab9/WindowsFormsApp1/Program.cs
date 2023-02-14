using ServiceStack.Redis.Generic;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Linq;
using System.Threading;
using System.Data.Linq.Mapping;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    internal class Program
    {
        static Form1 form;
        static string connectionString = "Data Source=localhost,1436;Initial Catalog=VideoGames;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static DataContext sqlServer = new DataContext(connectionString);
        static double count = 50.0;
        static GameSales redisEntity;
        [STAThread]
        private static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            form = new Form1();
            Thread.CurrentThread.CurrentCulture = customCulture;
            var sales = GetTop10();
            foreach (var s in sales)
                Console.WriteLine($"{s.title} {s.na_sales} {s.eu_sales} {s.jp_sales} {s.other_sales} {s.global_sales}");
            TimerCallback timerCallbackSelectFromSqlServer = new TimerCallback(SelectFromSqlServerTimerCallback);
            //SelectFromSqlServerTimerCallback(null);
            System.Threading.Timer timerSqlServer = new System.Threading.Timer(timerCallbackSelectFromSqlServer, null, 0, 5000);
            TimerCallback timerCallbackSelectFromRedis = new TimerCallback(SelectFromRedisTimerCallback);
            //SelectFromRedisTimerCallback(null);
            System.Threading.Timer timerRedis = new System.Threading.Timer(timerCallbackSelectFromRedis, null, 0, 5000);
            (long sql, long redis) selectRes, insertRes, deleteRes, updateRes;
            selectRes = insertRes = deleteRes = updateRes = (0, 0);
            for (int i = 0; i < count; i++)
            {
                var (sql, redis) = SelectTime();
                (selectRes.sql, selectRes.redis) = (selectRes.sql + sql, selectRes.redis + redis);
                (sql, redis) = InsertTime();
                (insertRes.sql, insertRes.redis) = (insertRes.sql + sql, insertRes.redis + redis);
                (sql, redis) = UpdateTime();
                (updateRes.sql, updateRes.redis) = (updateRes.sql + sql, updateRes.redis + redis);
                (sql, redis) = DeleteTime();
                (deleteRes.sql, deleteRes.redis) = (deleteRes.sql + sql, deleteRes.redis + redis);
            }
            form.AddColumn("SelSql", selectRes.sql / count );
            form.AddColumn("SelRed", selectRes.redis / count);
            form.AddColumn("InsSql", insertRes.sql / count);
            form.AddColumn("InsRed", insertRes.redis / count);
            form.AddColumn("DelSql", deleteRes.sql / count);
            form.AddColumn("DelRed", deleteRes.redis / count);
            form.AddColumn("UpdSql", updateRes.sql / count);
            form.AddColumn("UpdRed", updateRes.redis / count);
            Application.Run(form);
            
        }
        static IEnumerable<GameSales> GetTop10()
        {
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                IList<GameSales> gameSalesData = gameSalesR.GetAll();
                if (!gameSalesData.Any())
                {
                    var gameSalesSqlServer = from gs in sqlServer.GetTable<GameSales>() select gs;
                    gameSalesR.StoreAll(gameSalesSqlServer);
                }
                gameSalesData = gameSalesR.GetAll();
                var sales = (from gs in gameSalesData
                             orderby gs.global_sales descending
                             select gs).Take(10);
                return sales;
            }
        }
        static void SelectFromSqlServerTimerCallback(object obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select * from GameSales " +
                    "where global_sales > 40", connection);
                SqlDataReader r = command.ExecuteReader();
                Console.WriteLine("SqlServer select");
            }
        }
        static void SelectFromRedisTimerCallback(object obj)
        {
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                IList<GameSales> gameSalesData = gameSalesR.GetAll();
                if (!gameSalesData.Any())
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(
                            "select * from GameSales " +
                            "where global_sales > 40", connection);
                        SqlDataReader r = command.ExecuteReader();
                        if (r.HasRows)
                            while (r.Read())
                            {
                                GameSales gameSale = new GameSales
                                {
                                    title = (string)r["title"],
                                    na_sales = decimal.Parse(r["na_sales"].ToString()),
                                    eu_sales = decimal.Parse(r["eu_sales"].ToString()),
                                    jp_sales = decimal.Parse(r["jp_sales"].ToString()),
                                    other_sales = decimal.Parse(r["other_sales"].ToString()),
                                    global_sales = decimal.Parse(r["global_sales"].ToString())
                                };
                                gameSalesData.Add(gameSale);
                            }
                        gameSalesR.StoreAll(gameSalesData);
                    }
                gameSalesData = gameSalesR.GetAll();
                var sales = from gs in gameSalesData
                            where gs.global_sales > 40
                            select gs;
                Console.WriteLine("Redis select");
            }
        }
        static (long sql, long redis) SelectTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            long start1, start2, end1, end2;
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    stopwatch.Start();
                    start1 = stopwatch.ElapsedMilliseconds;
                    Table<GameSales> sqlServerTable = sqlServer.GetTable<GameSales>();
                    var gameSalesSqlServer = from gs in sqlServerTable
                                             where gs.global_sales > 40 
                                             select gs;
                    end1 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                    stopwatch.Start();
                    start2 = stopwatch.ElapsedMilliseconds;
                    IList<GameSales> gameSalesData = gameSalesR.GetAll();
                    end2 = stopwatch.ElapsedMilliseconds;
                    var sales = from gs in gameSalesData
                                where gs.global_sales > 40
                                select gs;
                    
                    stopwatch.Stop();
                }
            }
            return (end1 - start1, (end2 - start2));
        }
        static (long sql, long redis) InsertTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            Random r = new Random();
            decimal na = (decimal)(r.NextDouble() * 30), eu= (decimal)(r.NextDouble() * 30), jp = (decimal)(r.NextDouble() * 30), other = (decimal)(r.NextDouble() * 30);
            decimal global = na + eu + jp + other;
            long start1, start2, end1, end2;
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    stopwatch.Start();
                    start1 = stopwatch.ElapsedMilliseconds;
                    SqlCommand command = new SqlCommand(
                        "insert into GameSales " +
                        $"values ('Game123456', {na,0:0.00}, {eu,0:0.00}, {jp,0:0.00}, {other,0:0.00}, {global,0:0.00})", connection);
                    SqlDataReader read = command.ExecuteReader();
                    end1 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    read.Close();
                    /*command = new SqlCommand(
                        "select * from GameSales " +
                        "where title = 'Game123456'", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Console.Write("Sqlserver Insert ");
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write($"{reader[i]} ");
                        Console.WriteLine();
                    }*/
                    IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                    redisEntity = new GameSales
                    {
                        title = "Game123456",
                        na_sales = na,
                        eu_sales = eu,
                        jp_sales = jp,
                        other_sales = other,
                        global_sales = global
                    };
                    stopwatch.Start();
                    start2 = stopwatch.ElapsedMilliseconds;
                    gameSalesR.Store(redisEntity);
                    end2 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Stop();
                    /*var gs = from g in gameSalesR.GetAll()
                             where g.title == "Game123456"
                             select g;
                    if (gs.Any())
                        Console.WriteLine($"Insert Redis {gs.First().title} {gs.First().na_sales,0:0.00} {gs.First().eu_sales,0:0.00} {gs.First().jp_sales,0:0.00} {gs.First().other_sales,0:0.00} {gs.First().global_sales,0:0.00}");
                */}
            }
            return (end1 - start1, end2 - start2);
        }
        static (long sql, long redis) DeleteTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            long start1, start2, end1, end2;
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    stopwatch.Start();
                    start1 = stopwatch.ElapsedMilliseconds;
                    SqlCommand command = new SqlCommand(
                        "delete GameSales " +
                        $"where title = 'Game123456'", connection);
                    SqlDataReader read = command.ExecuteReader();
                    end1 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    read.Close();
                    /*command = new SqlCommand(
                        "select * from GameSales " +
                        "where title = 'Game123456'", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        Console.WriteLine("Successfully delteted form SqlServer");*/
                    IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                    stopwatch.Start();
                    start2 = stopwatch.ElapsedMilliseconds;
                    gameSalesR.Delete(redisEntity);
                    end2 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Stop();
                    /*var gs = from g in gameSalesR.GetAll()
                             where g.title == "Game123456"
                             select g;
                    if (!gs.Any())
                        Console.WriteLine("Successfully delteted form Redis");*/
                }
            }
            return (end1 - start1, end2 - start2);
        }
        static (long sql, long redis) UpdateTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            long start1, start2, end1, end2;
            using (RedisClient redisClient = new RedisClient("localhost", 6379))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    stopwatch.Start();
                    start1 = stopwatch.ElapsedMilliseconds;
                    SqlCommand command = new SqlCommand(
                        "update GameSales " +
                        "set eu_sales = eu_sales + 100, global_sales = global_sales + 100 "+
                        $"where title = 'Game123456'", connection);
                    SqlDataReader read = command.ExecuteReader();
                    end1 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    read.Close();
                    /*command = new SqlCommand(
                        "select * from GameSales " +
                        "where title = 'Game123456'", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.RecordsAffected == 1)
                    {
                        reader.Read();
                        Console.Write("Sqlserver Update ");
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write($"{reader[i]} ");
                        Console.WriteLine();
                    }*/
                    IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
                    stopwatch.Start();
                    start2 = stopwatch.ElapsedMilliseconds;
                    redisEntity.eu_sales += 100;
                    redisEntity.global_sales += 100;
                    gameSalesR.Store(redisEntity);
                    end2 = stopwatch.ElapsedMilliseconds;
                    stopwatch.Stop();
                    /*var gs = from g in gameSalesR.GetAll()
                             where g.title == "Game123456"
                             select g;
                    if (gs.Count() == 1)
                        Console.WriteLine($"Redis Update {gs.First().title} {gs.First().na_sales,0:0.00} {gs.First().eu_sales,0:0.00} {gs.First().jp_sales,0:0.00} {gs.First().other_sales,0:0.00} {gs.First().global_sales,0:0.00}");
                */}
            }
            return (end1 - start1, end2 - start2);  
        }

    }

    [Table]
    class GameSales
    {
        [Column(IsPrimaryKey = true)]
        public string title { get; set; }
        [Column]
        public decimal na_sales { get; set; }
        [Column]
        public decimal eu_sales { get; set; }
        [Column]
        public decimal jp_sales { get; set; }
        [Column]
        public decimal other_sales { get; set; }
        [Column]
        public decimal global_sales { get; set; }
    }
}
