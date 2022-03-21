using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace SMTP
{
    public static class SampleMailClient
    {
        public static async Task SendAsync(
            string from = null,
            string to = null,
            string subject = null,
            string user = null,
            string password = null,
            MimeEntity body = null,
            int count = 1,
            bool useSsl = false,
            int port = 25)
        {
            //var message = MimeMessage.Load(@"C:\Dev\Cain\Temp\message.eml");
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(from ?? "hmail@email-messenger.com"));
            message.To.Add(MailboxAddress.Parse(to ?? "przemyslaw.bak@simple-mail.net"));
            message.Subject = subject ?? "Hello";
            message.Body = body ?? new TextPart("plain")
            {
                Text = "Hello World body plain text"
            };

            using SmtpClient client = new SmtpClient();

            client.Connect("smtp.email-messenger.com", port, useSsl);

            if (user != null && password != null)
            {
                client.Authenticate(user, password);
            }

            while (count-- > 0)
            {
                await client.SendAsync(message);
            }

            client.Disconnect(true);
        }
    }
}
