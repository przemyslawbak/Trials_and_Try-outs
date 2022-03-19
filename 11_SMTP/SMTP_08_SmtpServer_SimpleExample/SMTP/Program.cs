using System;
using System.IO;
using System.Linq;

namespace SMTP
{
    //https://github.com/cosullivan/SmtpServer
    class Program
    {
        static void Main(string[] args)
        {
            SimpleExample.Run();

            var files = Directory.GetFiles(@"C:\Temp\enron_mail_20150507.tar", "*.*", SearchOption.AllDirectories).ToList();
            Console.WriteLine(files.OrderByDescending(file => new FileInfo(file).Length).First());
        }
    }
}
