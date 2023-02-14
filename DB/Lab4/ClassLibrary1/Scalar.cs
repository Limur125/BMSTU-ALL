using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ClassLibrary1
{
    public class Scalar
    {
        [SqlFunction]
        public static SqlInt32 GetRandomFromGap(SqlInt32 from, SqlInt32 to)
        {
            Random rnd = new Random();
            return (rnd.Next() % to + from);
        }
    }
}
