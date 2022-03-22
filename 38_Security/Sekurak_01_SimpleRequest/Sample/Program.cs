using System;
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
            HttpClient _client = new HttpClient();

            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Put;
                requestMessage.RequestUri = new Uri("https://kwejk.pl/");
                //requestMessage.Headers.Host = "training.securitum.com"; //throwing SSL exceptions
                requestMessage.Version = HttpVersion.Version11;
                requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0");
                requestMessage.Content = new StringContent("<?php phpinfo(); ?>", Encoding.UTF8, "text/html");

                Console.WriteLine("REUEST:");
                //Console.WriteLine("host: " + requestMessage.Headers.Host.ToString()); //throwing SSL exceptions
                Console.WriteLine("method: " + requestMessage.Method.ToString());
                Console.WriteLine("version: " + requestMessage.Version.ToString());
                Console.WriteLine("uri: " + requestMessage.RequestUri.ToString());
                Console.WriteLine("user-agent: " + requestMessage.Headers.UserAgent.ToString()); //??
                Console.WriteLine();
                Console.WriteLine("(...)");
                Console.WriteLine();
                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                Console.WriteLine("RESPONSE:");
                Console.WriteLine("status code: " + response.StatusCode);
                Console.WriteLine("version: " + (response.Version != null ? response.Version.ToString() : "not found"));
                Console.WriteLine("time: " + (response.Headers.Date != null ? response.Headers.Date.ToString() : "not found"));
                Console.WriteLine("server: " + (response.Headers.Server != null ? response.Headers.Server.ToString() : "not found"));
                Console.WriteLine("etag: " + (response.Headers.ETag != null ? response.Headers.ETag.ToString() : "not found"));
                Console.WriteLine("ranges: " + (response.Headers.AcceptRanges != null ? response.Headers.AcceptRanges.ToString() : "not found"));
                Console.WriteLine("content length: " + (response.Content.Headers.ContentLength != null ? response.Content.Headers.ContentLength.ToString() : "not found"));
                Console.WriteLine("content type: " + (response.Content.Headers.ContentType != null ? response.Content.Headers.ContentType.ToString() : "not found"));
                Console.WriteLine("content encoding: " + string.Join(",", response.Content.Headers.ContentEncoding.ToArray()));
                Console.WriteLine("allow: " + string.Join(",", response.Content.Headers.Allow.ToArray()));
                Console.WriteLine("html: " + await response.Content.ReadAsStringAsync());
            }
        }
    }
}
