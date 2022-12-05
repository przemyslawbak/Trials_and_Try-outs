using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 7-16. Observing WhenAll task exceptions

    public class Program
    {
        static void Main(string[] args)
        {
            //
        }

        public static async Task DownloadDocumentsWhenAny(params Uri[] downloads)
        {
            List<Task<string>> tasks = new List<Task<string>>();
            foreach (Uri uri in downloads)
            {
                var client = new WebClient();
                tasks.Add(client.DownloadStringTaskAsync(uri));
            }
            while (tasks.Count > 0)
            {
                Task<string> download =
                await Task.WhenAny(tasks);
                UpdateUI(download.Result);
                int nDownloadCompleted = tasks.IndexOf(download);
                Console.WriteLine("Downloaded {0}", downloads[nDownloadCompleted]);
                tasks.RemoveAt(nDownloadCompleted);
            }
        }

        private static void UpdateUI(string result)
        {
            throw new NotImplementedException();
        }
    }
}
