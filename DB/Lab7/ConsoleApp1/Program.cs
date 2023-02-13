using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LinqToObject lto = new LinqToObject();
            lto.LinkToObject();
            LinqToXml ltx = new LinqToXml();
            ltx.LinkToXml();
            LinqToSQL lts = new LinqToSQL();
            lts.LinkToSQL();
        }
    }
}
