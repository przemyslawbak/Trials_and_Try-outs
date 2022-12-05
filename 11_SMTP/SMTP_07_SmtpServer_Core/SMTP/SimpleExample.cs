using SmtpServer;
using SmtpServer.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP
{
    public class SimpleExample
    {
        public static void Run()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ISmtpServerOptions options = new SmtpServerOptionsBuilder()
                .ServerName("SmtpServer SampleApp")
                .Port(25)
                .Build();

            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.Add(new SampleMessageStore(Console.Out));

            SmtpServer.SmtpServer server = new SmtpServer.SmtpServer(options, serviceProvider);
            Task serverTask = server.StartAsync(cancellationTokenSource.Token);

            SampleMailClient.Send();

            cancellationTokenSource.Cancel();
            serverTask.Wait();
        }
    }
}
