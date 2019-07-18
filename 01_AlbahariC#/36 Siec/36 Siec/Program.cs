using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace _36_Siec
{
    class Program
    {
        static void Main(string[] args)
        {
            //adresy i porty
            IPAddress a1 = new IPAddress(new byte[] { 101, 102, 103, 104 });
            IPAddress a2 = IPAddress.Parse("101.102.103.104");
            Console.WriteLine(a1.Equals(a2)); // True
            Console.WriteLine(a1.AddressFamily); // InterNetwork
            IPAddress a3 = IPAddress.Parse
            ("[3EA0:FFFF:198A:E4A3:4FF2:54fA:41BC:8D31]");
            Console.WriteLine(a3.AddressFamily); // InterNetworkV6
            IPAddress a = IPAddress.Parse("101.102.103.104");
            IPEndPoint ep = new IPEndPoint(a, 222); // port 222
            Console.WriteLine(ep.ToString()); // 101.102.103.104 222

            //URI
            Uri info = new Uri("http://www.domain.com:80/info/");
            Uri page = new Uri("http://www.domain.com/info/page.html");
            Console.WriteLine(info.Host); // www.domain.com
            Console.WriteLine(info.Port); // 80
            Console.WriteLine(page.Port); // 80 (Uri zna domyślny port dla HTTP)
            Console.WriteLine(info.IsBaseOf(page)); // True
            Uri relative = info.MakeRelativeUri(page);
            Console.WriteLine(relative.IsAbsoluteUri); // False
            Console.WriteLine(relative.ToString()); // page.html

            //WebClient
            WebClient wc = new WebClient { Proxy = null };
            wc.DownloadFile("http://www.albahari.com/nutshell/code.aspx", "code.htm"); //jest też asynchroniczna wersja
            //System.Diagnostics.Process.Start("code.htm");
            Console.WriteLine("zgrano");

            //Klasy WebRequest i WebResponse
            WebRequest req = WebRequest.Create("http://www.albahari.com/nutshell/code.html");
            req.Proxy = null;
            using (WebResponse res = req.GetResponse()) //też jest wariant asynchroniczny
            using (Stream rs = res.GetResponseStream())
            using (FileStream fs = File.Create("code.html"))
                rs.CopyTo(fs);

            //uwierzytelnianie
            wc = new WebClient { Proxy = null };
            wc.BaseAddress = "ftp://ftp.albahari.com";
            // uwierzytelnienie, a następnie przekazanie pliku do serwera FTP i pobranie go stamtąd;
            // to samo podejście sprawdza się również w przypadku protokołów HTTP i HTTPS
            string username = "nutshell";
            string password = "oreilly";
            wc.Credentials = new NetworkCredential(username, password);
            wc.DownloadFile("guestbook.txt", "guestbook.txt");
            string data = "Pozdrowienia od " + Environment.UserName + "!\r\n";
            File.AppendAllText("guestbook.txt", data);
            wc.UploadFile("guestbook.txt", "guestbook.txt");

            //CredentialCache
            CredentialCache cache = new CredentialCache();
            Uri prefix = new Uri("http://exchange.somedomain.com");
            cache.Add(prefix, "Digest", new NetworkCredential("janek", "hasło"));
            cache.Add(prefix, "Negotiate", new NetworkCredential("janek", "hasło"));
            wc = new WebClient();
            wc.Credentials = cache;
        }
    }
}
