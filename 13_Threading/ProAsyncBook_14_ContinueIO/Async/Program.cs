using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-35. Continuing from an I/O-Based Task
    class Program
    {
        static void Main(string[] args)
        {
            //
        }

        private static Task<string> DownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            Task<WebResponse> response = request.GetResponseAsync();
            return response
            .ContinueWith<string>(grt =>
            {
                using (var reader = new StreamReader(grt.Result.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            });
        }
    }
}
