using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Schema;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=localhost,1436;Initial Catalog=rk3;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            DataContext db = new DataContext(connectionString);

            Console.WriteLine("\n\n\n1.");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "with tmp as " +
                    "( " +
                    "select fio, DATEDIFF(year, birthdate, getdate()) as age " +
                    "from employee " +
                    "where department = 'Fin' " +
                    ") " +
                    "select fio " +
                    "from tmp " +
                    "where age = (select max(age) from tmp)", connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
            var tmp1 = from e in db.GetTable<employee>()
                        where e.department == "Fin"
                        select new
                        {
                            fio = e.fio,
                            age = DateTime.Now.Date - e.birthdate
                        };
            var res1 = from e in tmp1.ToList()
                       where e.age == tmp1.ToList().Max(x => x.age)
                       select e;
            foreach (var r in res1)
            {
                Console.WriteLine($"{r.fio} {r.age}");
            }
            /*Console.WriteLine("\n\n\n2.");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "with tmp as " +
                    "( " +
                    "select fio, DATEDIFF(year, birthdate, getdate()) as age " +
                    "from employee " +
                    "where department = 'Fin' " +
                    ") " +
                    "select fio " +
                    "from tmp " +
                    "where age = (select max(age) from tmp)", connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
            var tmp2 = from e in db.GetTable<employee>()
                       where e.department == "Fin"
                       select new
                       {
                           fio = e.fio,
                           age = DateTime.Now.Date - e.birthdate
                       };
            var res2 = from e in tmp1.ToList()
                       where e.age == tmp1.ToList().Max(x => x.age)
                       select e;
            foreach (var r in res1)
            {
                Console.WriteLine($"{r.fio} {r.age}");
            }*/
            Console.WriteLine("\n\n\n3.");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "with temp as ( " +
                    "select min(lt.arrtime) as minartime, emp_id "+
                    "from logTable as lt "+
                    "where lt.arrtype = 1 "+
                    "group by emp_id)" +
                    "select fio " +
                    "from temp join employee as e on temp.emp_id = e.id " +
                    "where minartime = (select max(minartime) from temp)"
                    , connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
            var tmp3 = from e in db.GetTable<logTable>()
                       where e.arrtype == 1
                       group e.arrtime by e.emp_id;

            var tmp31 = from e in tmp3.ToList()
                        select new
                        {
                            miartime = e.ToList().Min(),
                            emp_id = e.ToList().First()
                       };
            var res3 = from e in tmp31.ToList()
                       where e.miartime == tmp31.ToList().Max(x => x.miartime)
                       select e;
            foreach(var r in res3)
            {
                Console.WriteLine($"{r.emp_id}");
            }
        }
        static void PrintQuery(SqlDataReader reader)
        {
            if (reader.HasRows)
                while (reader.Read())
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write($"{reader[i],20}\n");
        }
    }

    [Table]
    class employee
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        [Column]
        public string fio { get; set; }
        [Column]
        public DateTime birthdate { get; set; }
        [Column]
        public string department { get; set; }
    }

    [Table]
    class logTable
    {
        [Column]
        public int emp_id { get; set; }
        [Column]
        public DateTime rdate { get; set; }
        [Column]
        public string dayweek { get; set; }
        [Column]
        public TimeSpan arrtime { get; set; }
        [Column]
        public int arrtype { get; set; }
    }
}

