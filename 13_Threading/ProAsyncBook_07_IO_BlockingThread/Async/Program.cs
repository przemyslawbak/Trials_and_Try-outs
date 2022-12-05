using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-13. IO
    class Program
    {
        static void Main(string[] args)
        {
            string download = DownloadWebPage("https://www.rocksolidknowledge.com/"); //blockin thread
            Console.WriteLine(download);
        }
        private static string DownloadWebPage(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            {
                // this will return the content of the web page
                return reader.ReadToEnd();
            }
        }
    }
}
