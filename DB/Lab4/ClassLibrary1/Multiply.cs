using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;


namespace ClassLibrary1
{
    [Serializable]
    [SqlUserDefinedAggregate(Format.Native)]
    [StructLayout(LayoutKind.Sequential)]
    public struct Multiply
    {
        private int prod;

        public void Init()
        {
            prod = 1;
        }

        public void Accumulate(int value)
        {
            prod *= value;
        }

        public void Merge(Multiply Group)
        {
            prod *= Group.GetProductValue();
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