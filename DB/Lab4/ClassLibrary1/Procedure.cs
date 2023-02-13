using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Procedure
    {
        [SqlProcedure]
        public static void SelectCritics(int type)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select user_score from GameCritics where user_score > @score", connection);

                command.Parameters.AddWithValue("@score", type);

                SqlDataReader r = command.ExecuteReader();
                ((IDataRecord)r
                SqlContext.Pipe.Send(r);
            }
        }
    }
}
