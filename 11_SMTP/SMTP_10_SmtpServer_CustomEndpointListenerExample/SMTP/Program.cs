using System;
using System.IO;
using System.Linq;

namespace SMTP
{
    class Program
    {
        //https://github.com/cosullivan/SmtpServer
        static void Main(string[] args)
        {
            CustomEndpointListenerExample.Run();

            var files = Directory.GetFiles(@"C:\Temp\enron_mail_20150507.tar", "*.*", SearchOption.AllDirectories).ToList();
            Console.WriteLine(files.OrderByDescending(file => new FileInfo(file).Length).First());
        }
    }
}
