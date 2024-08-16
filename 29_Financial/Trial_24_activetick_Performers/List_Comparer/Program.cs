using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace List_Comparer
{
    class BookData
    {
        public Book[] book { get; set; }
    }

    class Book
    {
        public string x { get; set; }
        public int i { get; set; }
        public int f { get; set; }
        public string a { get; set; }
        public double p { get; set; }
        public int q { get; set; }
        public int o { get; set; }
        public string tm { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Use HttpClient to send a GET request to the API endpoint
            HttpClient client = new HttpClient();
            Task<string> response = client.GetStringAsync("https://api.activetick.com/performers.json?sessionid=acf9d3ce3e944261a27d5762f693a070&type=volume&source=nyse&columns=l,b,a,v,ltm");
            response.Wait();

            // Deserialize the response JSON into a dynamic object
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Result);

            // Check if the API returned 'ok' for the status
            if (data.status == "ok")
            {
                // Iterate through all rows of the response
                foreach (var row in data.rows)
                {
                    Console.WriteLine("Symbol: " + row.s + " | Status: " + row.st);
                    Console.WriteLine("Last: " + row.data[0].v + " | Bid: " + row.data[1].v + " | Ask: " + row.data[2].v + " | Volume: " + row.data[3].v + " | Last Timestamp: " + row.data[4].v);
                    Console.WriteLine("-------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("API returned an error: " + data.status);
            }
        }
    }
}
