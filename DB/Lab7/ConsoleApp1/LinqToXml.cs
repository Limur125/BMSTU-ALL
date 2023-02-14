using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class LinqToXml
    {
        public void LinkToXml()
        {
            XDocument xdoc = XDocument.Load(@"C:\zolot\DB\Lab7\ConsoleApp1\data2.xml");


            var items = from xe in xdoc.Elements("Games").Elements("Game")
                        where xe.Element("release").Value == "2000"
                        select new
                        {
                            Title = xe.Element("Game").Value,
                            Release = xe.Element("release").Value
                        };
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            xdoc = XDocument.Load(@"C:\zolot\DB\Lab7\ConsoleApp1\data2.xml");
            XElement root = xdoc.Element("Games");

            foreach (XElement xe in root.Elements("Game").ToList())
            {

                if (xe.Element("Game").Value == "\"Chaos;Child\"")
                {
                    xe.Element("release").Value = "2000";
                }
                if (xe.Element("Game").Value == "\"Chaos;Head\"")
                {
                    xe.Remove();
                }
            }

            root.Add(new XElement("Game",
                new XAttribute("Game", "Atomic Heart"),
                new XElement("release", "2023")));
            xdoc.Save(@"C:\zolot\DB\Lab7\ConsoleApp1\data.xml");

            Console.WriteLine("Xml изменен и сохранен");
        }
    }
}
