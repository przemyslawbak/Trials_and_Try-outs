using SmtpServer;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using SmtpServer.Protocol;
using System.Threading.Tasks;

namespace SMTP
{
    public static class DependencyInjectionExample
    {
        public static void Run()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ISmtpServerOptions options = new SmtpServerOptionsBuilder()
                .ServerName("SmtpServer SampleApp")
                .Port(9025)
                .Build();

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(options);
            services.AddTransient<ISmtpCommandFactory, CustomSmtpCommandFactory>();

            SmtpServer.SmtpServer server = new SmtpServer.SmtpServer(options, services.BuildServiceProvider());

            Task serverTask = server.StartAsync(cancellationTokenSource.Token);

            SampleMailClient.Send();

            cancellationTokenSource.Cancel();
            serverTask.Wait();
        }

        public sealed class CustomSmtpCommandFactory : SmtpCommandFactory
        {
            public override SmtpCommand CreateEhlo(string domainOrAddress)
            {
                return new CustomEhloCommand(domainOrAddress);
            }
        }

        public sealed class CustomEhloCommand : EhloCommand
        {
            public CustomEhloCommand(string domainOrAddress) : base(domainOrAddress) { }

            protected override string GetGreeting(ISessionContext context)
            {
                return "Good morning, Vietnam!";
            }
        }
    }
}
