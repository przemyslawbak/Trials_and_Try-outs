using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-16. IO async
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> downloadTask = BetterDownloadWebPageAsync45("https://www.rocksolidknowledge.com/"); //not blocking
            Console.WriteLine(downloadTask.Result);
        }

        private static async Task<string> BetterDownloadWebPageAsync45(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string result = await reader.ReadToEndAsync();

                return result;
            }

        }

        private static Task<string> BetterDownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            IAsyncResult ar = request.BeginGetResponse(null, null);
            Task<string> downloadTask = Task.Factory.FromAsync(ar, iar =>
            {
                using (WebResponse response = request.EndGetResponse(iar))
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });
            return downloadTask;
        }
    }
}
