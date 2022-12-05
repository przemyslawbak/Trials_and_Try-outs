using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using Users.Infrastructure;

namespace Users.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            MimeMessage msg = PrepareMesaage(message, subject, email);

            return SendViaClient(msg);
        }

        private Task SendViaClient(MimeMessage msg)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("smtp.wp.pl", 465, true);
                    client.Authenticate("przemyslaw.pszemek@wp.pl", "xxx");
                    client.Send(msg);
                    client.Disconnect(true);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private MimeMessage PrepareMesaage(string message, string subject, string email)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("4Sea Data", "przemyslaw.pszemek@wp.pl"));
            mailMessage.To.Add(new MailboxAddress("4Sea Data", email));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            return mailMessage;
        }
    }
}
