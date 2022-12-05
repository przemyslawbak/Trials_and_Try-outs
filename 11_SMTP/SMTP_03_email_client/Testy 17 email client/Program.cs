using OpenPop.Mime;
using OpenPop.Pop3;
using System;

namespace Testy_17_email_client
{
    class Program
    {
        //OpenPop.NET NuGet library
        static void Main(string[] args)
        {
            var client = new Pop3Client();
            client.Connect("pop.simple-mail.net", 110, false);
            client.Authenticate("przemyslaw.bak@simple-mail.net", "666");
            var count = client.GetMessageCount();
            Message message = client.GetMessage(count);
            Console.WriteLine(count.ToString());
            Console.WriteLine(message.Headers.Subject);

            Console.ReadKey();
        }
    }
}
