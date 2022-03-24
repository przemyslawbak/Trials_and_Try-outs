using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string host = "training.securitum.com";
            string path = "/cookie.php";

            //for CURL https://stackoverflow.com/a/43975415/12603542
            //https://stackoverflow.com/a/56460052/12603542
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient _client = new HttpClient(clientHandler); //todo: static client
            _client.BaseAddress = new Uri("https://" + host);

            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, path))
            {
                requestMessage.Method = HttpMethod.Get;
                requestMessage.Headers.Host = host; //host changed
                requestMessage.Version = HttpVersion.Version11;
                requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0");
                requestMessage.Headers.Add("Location", "https://kwejk.pl/");
                //requestMessage.Headers.Referrer = new Uri("https://www.google.com/");
                requestMessage.Content = new StringContent("<?php phpinfo(); ?>", Encoding.UTF8, "text/html");

                Console.WriteLine("REUEST:");
                Console.WriteLine("method: " + requestMessage.Method.ToString());
                Console.WriteLine("version: " + requestMessage.Version.ToString());
                Console.WriteLine("host: " + (requestMessage.Headers.Host != null ? requestMessage.Headers.Host.ToString() : "not found"));
                Console.WriteLine("relative uri: " + requestMessage.RequestUri.ToString());
                Console.WriteLine("referer: " + (requestMessage.Headers.Referrer != null ? requestMessage.Headers.Referrer.ToString() : "not found"));
                Console.WriteLine("user-agent: " + (requestMessage.Headers.UserAgent != null ? requestMessage.Headers.UserAgent.ToString() : "not found"));
                Console.WriteLine("connection: " + (requestMessage.Headers.Connection != null ? requestMessage.Headers.Connection.ToString() : "not found"));
                Console.WriteLine("accept: " + (requestMessage.Headers.Accept != null ? requestMessage.Headers.Accept.ToString() : "not found"));
                Console.WriteLine("accept-lang: " + (requestMessage.Headers.AcceptLanguage != null ? requestMessage.Headers.AcceptLanguage.ToString() : "not found"));
                Console.WriteLine("upgrade: " + (requestMessage.Headers.Upgrade != null ? requestMessage.Headers.Upgrade.ToString() : "not found"));
                Console.WriteLine();
                Console.WriteLine("(...)");
                Console.WriteLine();
                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;
                Console.WriteLine("RESPONSE:");
                Console.WriteLine("status code: " + response.StatusCode);
                Console.WriteLine("version: " + (response.Version != null ? response.Version.ToString() : "not found"));
                Console.WriteLine("time: " + (response.Headers.Date != null ? response.Headers.Date.ToString() : "not found"));
                Console.WriteLine("server: " + (response.Headers.Server != null ? response.Headers.Server.ToString() : "not found"));
                Console.WriteLine("etag: " + (response.Headers.ETag != null ? response.Headers.ETag.ToString() : "not found"));
                Console.WriteLine("vary: " + (response.Headers.Vary != null ? response.Headers.Vary.ToString() : "not found"));
                Console.WriteLine("ranges: " + (response.Headers.AcceptRanges != null ? response.Headers.AcceptRanges.ToString() : "not found"));
                Console.WriteLine("content length: " + (response.Content.Headers.ContentLength != null ? response.Content.Headers.ContentLength.ToString() : "not found"));
                Console.WriteLine("content type: " + (response.Content.Headers.ContentType != null ? response.Content.Headers.ContentType.ToString() : "not found"));
                Console.WriteLine("content encoding: " + string.Join(",", response.Content.Headers.ContentEncoding.ToArray()));
                Console.WriteLine("allow: " + string.Join(",", response.Content.Headers.Allow.ToArray()));
                Console.WriteLine("cookies: " + string.Join(",", (cookies != null ? cookies.ToArray() : "not found" )));
                Console.WriteLine("connection: " + (response.Headers.Connection != null ? response.Headers.Connection.ToString() : "not found"));
                Console.WriteLine("html: " + await response.Content.ReadAsStringAsync());
            }
        }
    }
}
