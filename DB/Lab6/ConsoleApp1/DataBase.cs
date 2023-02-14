using System;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    internal class DataBase
    {
        string connectionString = "Data Source=localhost,1436;Initial Catalog=VideoGames;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void ScalarQuery()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select avg(user_score) as [AverageUserScore]" +
                    "from GameCritics " +
                    "where user_score > 4", connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        void PrintQuery(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                Console.Write("|");
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader.GetName(i),40}\t|");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.Write("|");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        object f = reader.GetValue(i);
                        Console.Write($"{f,40}\t|");
                    }
                    Console.WriteLine();
                }
            }
        }
        public void JoinQuery()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select OD.title, developer, OD.crit_score " +
                    "from VideoGames as V " +
                    "join " +
                    "( " +
                        "select top 1000 GameCritics.title, crit_score " +
                        "from GameCritics " +
                        "order by crit_score desc " +
                    ") " +
                    "as OD on OD.title = V.title",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void CTEWindowQuery()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "with idSales as " +
                    "( " +
                        "select gs.*, row_number() over(order by title asc) as id " +
                        "from GameCritics as gs " +
                    "), " +
                    "RecursiveCrits as " +
                    "( " +
                        "select id, title, crit_score, cast(crit_score as numeric(30)) as [mul] " +
                        "from idSales as l " +
                        "where id = 1 " +
                        "union all " +
                        "select l.id, l.title, l.crit_score, cast([mul] * rec_l.crit_score as numeric(30)) " +
                        "from idSales as l " +
                        "join RecursiveCrits as rec_l on l.id - 1 = rec_l.id " +
                        "where rec_l.id < 6 " +
                    ") " +
                    "select * " +
                    "from RecursiveCrits",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void MetaQuery()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select count(*) as IntCount " +
                    "from(sys.all_columns as c join sys.types as t on c.user_type_id = t.user_type_id) " +
                    "join sys.objects as o on c.object_id = o.object_id " +
                    "where t.name = 'int' and type = 'U' " +
                    "group by t.user_type_id",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void ScalarFunction()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select title, jp_sales, global_sales, cast(dbo.fraction(jp_sales, global_sales) as varchar(5)) + '%' as [percentage] " +
                    "from GameSales " +
                    "where jp_sales > 6",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void TableFunction()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select * " +
                    "from game_table('M')",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void Procedure()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "exec recursivproc",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void SystemFunction()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select title, developer, " +
                    "case release " +
                        "when 2022 THEN 'This Year'" +
                        "when 2021 THEN 'Last year'" +
                        "else cast(2022 - release as varchar(5)) + ' years ago' " +
                    "end as 'When' " +
                    "from VideoGames",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "drop table if exists XmlVideoGames", connection);
                SqlDataReader r = command.ExecuteReader();
                r.Close();
                command = new SqlCommand(
                    "create table XmlVideoGames " +
                    "( " +
                    "[title]     VARCHAR(200) NOT NULL, " +
                    "[release]   INT           NOT NULL, " +
                    "[developer] VARCHAR(100) NOT NULL, " +
                    "[publisher] varchar(100) NOT NULL, " +
                    "[genre]     xml NOT NULL " +
                    ")", connection);
                r = command.ExecuteReader();
            }
        }
        public void ВставкаДанных()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "declare @fileDataX xml " +
                    "select @fileDataX = BulkColumn " +
                    "from openrowset " +
                    "( " +
                        "bulk '\\data2.xml', " +
                        "single_blob " +
                    ") x; " +
                    "insert into XmlVideoGames([title], [release], [developer], [publisher], [genre]) " +
                    "select " +
                        "xData.value('Game[1]', 'varchar(200)') title, " +
                        "xData.value('release[1]', 'int')[release], " +
                        "xData.value('developer[1]', 'varchar(100)')[developer], " +
                        "xData.value('publisher[1]', 'varchar(100)')[publisher], " +
                        "xData.query('Genres[1]')[genre] " +
                    "from @fileDataX.nodes('Games/Game') as x(xData) " +
                    "select * from XmlVideoGames ",
                    connection);

                SqlDataReader r = command.ExecuteReader();
                PrintQuery(r);
            }
        }
    }
}
