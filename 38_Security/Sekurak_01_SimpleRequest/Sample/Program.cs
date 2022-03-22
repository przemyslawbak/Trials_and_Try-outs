using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0");

            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://training.securitum.com/"))
            {
                Console.WriteLine("REUEST:");
                Console.WriteLine("method: " + requestMessage.Method.ToString());
                Console.WriteLine("uri: " + requestMessage.RequestUri.ToString());
                Console.WriteLine("user-agent: " + requestMessage.Headers.UserAgent.ToString());
                Console.WriteLine();
                Console.WriteLine("(...)");
                Console.WriteLine();
                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                Console.WriteLine("RESPONSE:");
                Console.WriteLine("status code: " + response.StatusCode.ToString());
                Console.WriteLine("version: " + response.Version.ToString());
                Console.WriteLine("time: " + response.Headers.Date.ToString());
                Console.WriteLine("server: " + response.Headers.Server.ToString());
                Console.WriteLine("etag: " + response.Headers.ETag.ToString());
                Console.WriteLine("ranges: " + response.Headers.AcceptRanges.ToString());
                Console.WriteLine("content length: " + response.Content.Headers.ContentLength.ToString());
                Console.WriteLine("content type: " + response.Content.Headers.ContentType.ToString());
                Console.WriteLine("content encoding: " + response.Content.Headers.ContentEncoding.ToString());
                Console.WriteLine("html: " + await response.Content.ReadAsStringAsync());
            }
        }
    }
}
