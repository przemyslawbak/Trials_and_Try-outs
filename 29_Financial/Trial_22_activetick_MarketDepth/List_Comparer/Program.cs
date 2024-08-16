using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;

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
            var url = "https://api.activetick.com/book_snapshot.json";
            var client = new HttpClient();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("sessionid", "acf9d3ce3e944261a27d5762f693a070"),
                new KeyValuePair<string, string>("symbol", "MSFT_S U"),
                new KeyValuePair<string, string>("source", "totalview"),
            });

            var response = client.PostAsync(url, data).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var bookData = JsonConvert.DeserializeObject<BookData>(content);

            foreach (var book in bookData.book)
            {
                Console.WriteLine("Side: " + book.x + " | Index: " + book.i + " | Flags: " + book.f + " | Attribution: " + book.a + " | Price: " + book.p + " | Quantity: " + book.q + " | Orders: " + book.o + " | Timestamp: " + book.tm);
            }

            Console.ReadLine();
        }
    }
}
