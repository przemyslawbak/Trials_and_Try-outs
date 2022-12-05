using SmtpServer;
using SmtpServer.ComponentModel;
using SmtpServer.Tracing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP
{
    public static class SimpleServerExample
    {
        public static void Run()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ISmtpServerOptions options = new SmtpServerOptionsBuilder()
                .ServerName("SmtpServer SampleApp")
                .Port(9025)
                .CommandWaitTimeout(TimeSpan.FromSeconds(100))
                .Build();

            SmtpServer.SmtpServer server = new SmtpServer.SmtpServer(options, ServiceProvider.Default);
            server.SessionCreated += OnSessionCreated;

            Task serverTask = server.StartAsync(cancellationTokenSource.Token);

            Console.WriteLine("Press any key to shutdown the server.");
            Console.ReadKey();

            cancellationTokenSource.Cancel();
            serverTask.Wait();
        }

        static void OnSessionCreated(object sender, SessionEventArgs e)
        {
            Console.WriteLine("Session Created.");

            e.Context.CommandExecuting += OnCommandExecuting;
        }

        static void OnCommandExecuting(object sender, SmtpCommandEventArgs e)
        {
            Console.WriteLine("Command Executing.");

            new TracingSmtpCommandVisitor(Console.Out).Visit(e.Command);
        }
    }
}
