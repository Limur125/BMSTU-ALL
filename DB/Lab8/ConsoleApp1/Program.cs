using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

class Program
{ 
    static void Main()
    {
        XDocument xdoc = XDocument.Load(@"C:\zolot\DB\Lab7\ConsoleApp1\data2.xml");


        var items = from xe in xdoc.Elements("Games").Elements("Game") select xe;
        int k = 0;
        int i = 0;
        XmlWriter xmlWriter = XmlWriter.Create(@$"C:\zolot\DB\Lab8\nifi\in_file\data\{i++}_labeight_{DateTime.Now.Day}.xml");
        foreach (var item in items)
        {
            XElement xelem = new XElement("game", new XElement("title", item.Element("Game")?.Value),
                new XElement("release", item.Element("release")?.Value),
                new XElement("developer", item.Element("developer")?.Value),
                new XElement("publisher", item.Element("publisher")?.Value));
            xelem.WriteTo(xmlWriter);
            if (k % 5 == 0)
            {
                xmlWriter.Close();
                Thread.Sleep(3000);
                xmlWriter = XmlWriter.Create(@$"C:\zolot\DB\Lab8\nifi\in_file\data\{i++}_labeight_{DateTime.Now.Day}.xml");
            }
        }
        xmlWriter.Close();
    }
}
