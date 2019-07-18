using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.ComponentModel;

namespace Testy_03_SMTP_console_app
{
    class Program
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;

            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            bool flag1 = p.SendEmail1();
            if (flag1)
            {
                Console.WriteLine("OK");
            }



            Console.ReadKey();
        }

        public bool SendEmail1()
        {
            MailMessage msg = new MailMessage();
            msg.To.Add("przemyslaw.pszemek@wp.pl");//Mail recipient account
            msg.From = new MailAddress("przemyslaw.bak@pro-emailing.net", "PszemekB", System.Text.Encoding.UTF8);//Mail account and displays the name and the character encoding
            msg.Subject = "Moj pierwszy program do wysylania maili";//Message header
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//Mail title emcoding
            msg.Body = "tresc wiadomosci. dostales ja?";//The message content
            msg.BodyEncoding = System.Text.Encoding.UTF8;//Message encoding
            msg.IsBodyHtml = false;//Whether the HTML mail
            msg.Priority = MailPriority.Normal;//Priority mail
            //msg.Attachments.Add(new Attachment("costam"));

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("przemyslaw.bak@pro-emailing.net", "Halabanaha11");//The registered email address and password, the QQ mailbox, if you set a password to use independent, independent password instead of the password
            client.Host = "smtp.pro-emailing.net";//QQ mailbox corresponding to the SMTP server
            client.Port = 587;
            string userState = "test message1";
            try
            {
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                client.SendAsync(msg, userState);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
