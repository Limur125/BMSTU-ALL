using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;
using System.Security;

namespace ConsoleApp1
{
    internal class LinqToSQL
    {
        public void LinkToSQL()
        {
            string connection = "Data Source=localhost,1436;Initial Catalog=VideoGames;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            DataContext db = new DataContext(connection);

            ////Однотабличный запрос на выборку.
            Console.WriteLine("\n\n\n1.");
            var users = from u in db.GetTable<GameSales>()
                        where u.global_sales < 15.0m
                        orderby u.title
                        select u;
            foreach (var user in users)
            {
                Console.WriteLine($"{user.title} {user.global_sales}");
            }

            //Многотабличный запрос на выборку.
            Console.WriteLine("\n\n\n2.");
            var comps = from vg in db.GetTable<VideoGames>()
                        join gc in db.GetTable<GameCritics>() on vg.title equals gc.title
                        select new { Title = vg.title, Developer = vg.developer, Platform = gc.platform };
            foreach (var user in comps)
            {
                Console.WriteLine(user);
            }

            // Изменение
            Console.WriteLine("\n\n\n3.");
            var userdb = from p in db.GetTable<Platforms>()
                         where p.name == "PC"
                         select p;
            userdb.First().RAM = 8000;
            Console.WriteLine($"Старое {userdb.First().RAM}");
            userdb.First().RAM = 16000;
            db.SubmitChanges();
            var userdb2 = db.GetTable<Platforms>().Where(c => c.name == "PC").FirstOrDefault();
            Console.WriteLine($"Новое {userdb2.name} {userdb2.RAM}");

            var userdb1 = db.GetTable<Platforms>();
            Console.WriteLine("Вставка");
            Platforms plat = new Platforms()
            {
                name = "efnisksmmt",
                company = "nfnwanckm",
                rel_year = 2029,
                processor = 487,
                RAM = 982,
            };
            userdb1.InsertOnSubmit(plat);
            db.SubmitChanges();
            Console.WriteLine("Вставка завершена");

            var query = from gc in db.GetTable<Platforms>()
                        where gc.name == "efnisksmmt"
                        select gc;
            foreach (var g in query)
                Console.WriteLine($"{g.name} {g.company} {g.rel_year} {g.processor} {g.RAM}");

            var usert2 = db.GetTable<Platforms>().Where(c => c.name == "efnisksmmt").FirstOrDefault();
            db.GetTable<Platforms>().DeleteOnSubmit(usert2);
            db.SubmitChanges();
            Console.WriteLine("Объект удален");
            var query2 = from gc in db.GetTable<Platforms>()
                         where gc.name == "efnisksmmt"
                         select gc;
            foreach (var g in query2)
                Console.WriteLine($"{g.name} {g.company} {g.rel_year} {g.processor} {g.RAM}");



            Procedure db1 = new Procedure(connection);

            decimal a = 10, b = 20;

            Console.WriteLine($"{db1.CountTypes(a, b)}%");
        }
    }
    public class Procedure : DataContext
    {
        public Procedure(string connectionString) : base(connectionString) { }

        [Function(Name = "dbo.fraction")]
        [return: Parameter(DbType = "Decimal")]
        public int CountTypes(
            [Parameter(Name = "region_sales", DbType = "Decimal")] decimal reg_s,
            [Parameter(Name = "global_sales", DbType = "Decimal")] decimal glob_s
            )
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), reg_s, glob_s);
            return ((int)(result.ReturnValue));
        }
    }
    [Table]
    class VideoGames
    {
        [Column(IsPrimaryKey = true)]
        public string title { get; set; }
        [Column]
        public int release { get; set; }
        [Column]
        public string developer { get; set; }
        [Column]
        public string publisher { get; set; }
        [Column]
        public string genre { get; set; }
    }
    [Table]
    class GameCritics
    {
        [Column(IsPrimaryKey = true)]
        public string title { get; set; }
        [Column]
        public string platform { get; set; }
        [Column]
        public string summary { get; set; }
        [Column]
        public int crit_score { get; set; }
        [Column]
        public decimal user_score { get; set; }
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
    [Table]
    class Platforms
    {
        [Column(IsPrimaryKey = true)]
        public string name { get; set; }
        [Column]
        public string company { get; set; }
        [Column]
        public int rel_year { get; set; }
        [Column]
        public int processor { get; set; }
        [Column]
        public int RAM { get; set; }
    }

}
