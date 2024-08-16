using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;

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
            // Define session ID, symbols and columns
            string sessionId = "acf9d3ce3e944261a27d5762f693a070";
            string symbols = "AAPL_S-U,IBM_S-U";
            string columns = "pc,l,b,a,bsz,asz,v,ltm";

            // Define snapshot API endpoint URL
            string url = "https://api.activetick.com/snapshot.json";

            // Make a GET request to snapshot API endpoint
            HttpClient client = new HttpClient();
            var queryString = "sessionid=" + sessionId + "&symbols=" + symbols + "&columns=" + columns;
            var response = client.GetAsync(url + "?" + queryString).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                JObject o = JObject.Parse(json);

                // Iterate through response rows
                foreach (var row in o["rows"])
                {
                    Console.WriteLine("Symbol: " + row["symbol"]);
                    Console.WriteLine("  Status: " + row["status"]);

                    // Iterate through data in each row
                    foreach (var data in row["data"])
                    {
                        string columnName = (string)data["c"];
                        if (columnName == "pc")
                            Console.WriteLine("\tPrevious Close: " + data["v"]);
                        else if (columnName == "l")
                            Console.WriteLine("\tLast Trade Price: " + data["v"]);
                        else if (columnName == "b")
                            Console.WriteLine("\tBid Price: " + data["v"]);
                        else if (columnName == "a")
                            Console.WriteLine("\tAsk Price: " + data["v"]);
                        else if (columnName == "bsz")
                            Console.WriteLine("\tBid Size: " + data["v"]);
                        else if (columnName == "asz")
                            Console.WriteLine("\tAsk Size: " + data["v"]);
                        else if (columnName == "vol")
                            Console.WriteLine("\tVolume: " + data["v"]);
                        else if (columnName == "ltm")
                            Console.WriteLine("\tLast Trade Time: " + data["v"]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}
