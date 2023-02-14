using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ClassLibrary1
{
    [Serializable]
    [SqlUserDefinedAggregateAttribute(Format.Native)]
    [StructLayout(LayoutKind.Sequential)]
    public struct Def
    {
        private int prod;

        public void Init()
        {
            prod = 0;
        }

        public void Accumulate(SqlString text)
        {
            prod++;
        }

        public void Merge(Def Group)
        {
            prod += Group.GetProductValue();
        }

        public int Terminate()
        {
            return prod;
        }

        //  Helper methods
        private int GetProductValue()
        {
            return prod;
        }
    }
}