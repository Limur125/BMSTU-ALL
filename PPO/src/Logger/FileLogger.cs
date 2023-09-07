using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class FileLogger : ILogger
    {
        public void Print(string message)
        {
            using StreamWriter stream = new StreamWriter($"{DateTime.Today.ToShortDateString()}_log.txt", true);
            stream.WriteLine($"{DateTime.Now} {message}");
        }
    }
}
