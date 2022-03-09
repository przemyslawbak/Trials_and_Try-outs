//https://github.com/cosullivan/SmtpServer
using SmtpServer;
using SmtpServer.Authentication;
using SmtpServer.ComponentModel;
using SmtpServer.Mail;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP
{
    public class MailFilter : IMailboxFilter
    {
        public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox @from, int size, CancellationToken cancellationToken)
        {
            if (String.Equals(@from.Host, "test.com"))
            {
                return Task.FromResult(MailboxFilterResult.Yes);
            }

            return Task.FromResult(MailboxFilterResult.NoPermanently);
        }

        public Task<MailboxFilterResult> CanDeliverToAsync(ISessionContext context, IMailbox to, IMailbox @from, CancellationToken token)
        {
            return Task.FromResult(MailboxFilterResult.Yes);
        }

        public IMailboxFilter CreateInstance(ISessionContext context)
        {
            return new MailFilter();
        }
    }

    public class SampleMessageStore : MessageStore
    {
        public override async Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            await using var stream = new MemoryStream();

            var position = buffer.GetPosition(0);
            while (buffer.TryGet(ref position, out var memory))
            {
                await stream.WriteAsync(memory, cancellationToken);
            }

            stream.Position = 0;

            var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);
            Console.WriteLine(message.TextBody);

            return SmtpResponse.Ok;
        }
    }

    public class SampleUserAuthenticator : IUserAuthenticator, IUserAuthenticatorFactory
    {
        public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken token)
        {
            Console.WriteLine("User={0} Password={1}", user, password);

            return Task.FromResult(user.Length > 4);
        }

        public IUserAuthenticator CreateInstance(ISessionContext context)
        {
            return new SampleUserAuthenticator();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("localhost")
                .Port(25, 587)
                .Port(465, isSecure: true)
                .Build();

            var serviceProvider = new ServiceProvider();
            serviceProvider.Add(new SampleMessageStore());
            serviceProvider.Add(new MailFilter());
            serviceProvider.Add(new SampleUserAuthenticator());

            var smtpServer = new SmtpServer.SmtpServer(options, serviceProvider);
            await smtpServer.StartAsync(CancellationToken.None);
        }

        
    }
}
