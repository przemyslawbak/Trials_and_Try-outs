using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extractor
{
    class Program
    {
        private static readonly string _sep = "\t";

        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string address = "Berlin";
            var apikey = "<KEY>";
            var url = "https://geocoder.ls.hereapi.com/6.2/geocode.json?apiKey=";
            var fullurl = url + apikey + "&searchtext=" + address;
            // Create a request for the URL.        
            WebRequest request = WebRequest.Create(fullurl);
            request.Method = "GET";
            request.ContentType = "application/json";
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader rrr = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = rrr.ReadToEnd();
        }
    }
}
