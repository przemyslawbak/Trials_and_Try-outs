using MailKit.Net.Smtp;
using MimeKit;

namespace SMTP_04_MailKit
{
    class Program
    {
        public static void Main(string[] args)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("4Sea Data - Support", "przemyslaw.pszemek@wp.pl"));
            mailMessage.To.Add(new MailboxAddress("4Sea Data - Support", "przemyslaw.pszemek@wp.pl"));
            mailMessage.Subject = "Subject";
            mailMessage.ReplyTo.Add(new MailboxAddress("Some Name", "email@gmail.com"));
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Text"
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.wp.pl", 465, true);

                client.Authenticate("przemyslaw.pszemek@wp.pl", "xxx");

                client.Send(mailMessage);
                client.Disconnect(true);
            }
        }
    }
}
