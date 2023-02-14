using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;


namespace ClassLibrary1
{
    public class Table
    {
        [SqlFunction(FillRowMethodName = "SplitStringFillRow", TableDefinition = "part VARCHAR(200), ID_ORDER INT")]

        static public IEnumerator CountGames(SqlString text)
        {
            if (text.IsNull) yield break;

            int valueIndex = 1;
            foreach (string s in text.Value.Split(delimiter, StringSplitOptions.RemoveEmptyEntries))
            {
                yield return new KeyValuePair<int, string>(valueIndex++, s.Trim());
            }
        }

        static public void SplitStringFillRow(object oKeyValuePair, out SqlString value, out SqlInt32 valueIndex)
        {
            KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)oKeyValuePair;

            valueIndex = keyValuePair.Key;
            value = keyValuePair.Value;
        }
    }
}
