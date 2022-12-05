using MailKit.Net.Smtp;
using MimeKit;

namespace SMTP
{
    public static class SampleMailClient
    {
        public static void Send(
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

            message.From.Add(MailboxAddress.Parse(from ?? "from@sample.com"));
            message.To.Add(MailboxAddress.Parse(to ?? "przemyslaw.bak@simple-mail.net"));
            message.Subject = subject ?? "Hello World";
            message.Body = body ?? new TextPart("plain")
            {
                Text = "Email body of Hello World message"
            };

            using SmtpClient client = new SmtpClient();

            client.Connect("localhost", port, useSsl);

            if (user != null && password != null)
            {
                client.Authenticate(user, password);
            }

            while (count-- > 0)
            {
                client.Send(message);
            }

            client.Disconnect(true);
        }
    }
}
