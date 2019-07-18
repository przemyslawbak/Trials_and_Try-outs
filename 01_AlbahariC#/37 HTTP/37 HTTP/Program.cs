using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _37_HTTP
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient { Proxy = null };
            wc.Headers.Add("CustomHeader", "JustPlaying/1.0");
            wc.DownloadString("http://www.oreilly.com");
            foreach (string name in wc.ResponseHeaders.Keys)
                Console.WriteLine(name + "=" + wc.ResponseHeaders[name]);
            //ciąg tekstowy zapytań
            wc = new WebClient { Proxy = null };
            wc.QueryString.Add("q", "WebClient"); // wyszukiwanie słowa "WebClient"
            wc.QueryString.Add("hl", "pl"); // wyświetlenie strony w języku polskim
            wc.DownloadFile("https://www.wp.pl/", "results.html");

            //przekazywanie danych formularza
            wc = new WebClient { Proxy = null }; //jest też przykład z WebRequest, ale pomijam
            var data = new System.Collections.Specialized.NameValueCollection();
            data.Add("Name", "Joe Albahari");
            data.Add("Company", "O'Reilly");
            byte[] result = wc.UploadValues("http://www.albahari.com/EchoPost.aspx",
            "POST", data);
            Console.WriteLine(Encoding.UTF8.GetString(result));

            //Paweł Cookiz
            var cc = new CookieContainer();
            var request = (HttpWebRequest)WebRequest.Create("http://www.google.pl"); //można też z HttpClient ale pomijam przykład
            request.Proxy = null;
            request.CookieContainer = cc;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Console.WriteLine("Ciasto:");
                foreach (Cookie c in response.Cookies)
                {
                    Console.WriteLine(" Nazwa: " + c.Name);
                    Console.WriteLine(" Wartość: " + c.Value);
                    Console.WriteLine(" Ścieżka: " + c.Path);
                    Console.WriteLine(" Domena: " + c.Domain);
                }
                // odczyt strumienia odpowiedzi...
            }

            //uwierzytelnianie na podstawie formularzy - dobre, do przerobienia jeszcze raz s683, jest wersja dla HttpClient i WebRequest

            //użycie FTP
            //nie da się połączyć z servem :(
            /*
            wc = new WebClient { Proxy = null };
            wc.Credentials = new NetworkCredential("nutshell", "oreilly");
            wc.BaseAddress = "ftp://ftp.albahari.com";
            wc.UploadString("tempfile.txt", "Witaj!");
            Console.WriteLine(wc.DownloadString("tempfile.txt")); // Witaj!
            Console.WriteLine("pobrany");
            */

            //DNS
            foreach (IPAddress a in Dns.GetHostAddresses("albahari.com"))
                Console.WriteLine(a.ToString()); // 64.68.200.46
            IPHostEntry entry = Dns.GetHostEntry("64.68.200.46");
            Console.WriteLine(entry.HostName); // albahari.com
            foreach (IPAddress a in Dns.GetHostAddresses("przemyslaw-bak.pl"))
                Console.WriteLine(a.ToString());

            //SMTP
            SmtpClient client = new SmtpClient();
            client.Host = "mail.myisp.net";
            MailMessage mm = new MailMessage();
            mm.Sender = new MailAddress("kasia@nazwa_domeny.pl", "Kasia");
            mm.From = new MailAddress("kasia@nazwa_domeny.pl", "Kasia");
            mm.To.Add(new MailAddress("bartek@nazwa_domeny.pl", "Bartek"));
            mm.CC.Add(new MailAddress("danka@nazwa_domeny.pl", "Danka"));
            mm.Subject = "Witaj!";
            mm.Body = "Witaj. Oto zdjęcie!";
            mm.IsBodyHtml = false;
            mm.Priority = MailPriority.High;
            //Attachment b = new Attachment("zdjęcie.jpg", System.Net.Mime.MediaTypeNames.Image.Jpeg);
            //mm.Attachments.Add(b);
            //client.Send(mm); <- przerobione

            //TCP
            new Thread(Server).Start(); // współbieżne uruchomienie metody serwera
            Thread.Sleep(500); // serwer otrzymuje nieco czasu na uruchomienie
            Client();

            //POP3

            Console.ReadKey();
        }
        //client TCP
        static void Client()
        {
            using (TcpClient client = new TcpClient("localhost", 51111))
            using (NetworkStream n = client.GetStream())
            {
                BinaryWriter w = new BinaryWriter(n);
                w.Write("Witaj");
                w.Flush();
                Console.WriteLine(new BinaryReader(n).ReadString());
            }
        }
        //serwer TCP
        static void Server() // obsługuje pojedyncze żądanie klienta, a następnie kończy działanie
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            using (TcpClient c = listener.AcceptTcpClient())
            using (NetworkStream n = c.GetStream())
            {
                string msg = new BinaryReader(n).ReadString();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(msg + " ponownie");
                w.Flush(); // konieczne jest wywołanie Flush(), ponieważ
            } // nie pozbywamy się komponentu
            listener.Stop();
        }
    }
}
