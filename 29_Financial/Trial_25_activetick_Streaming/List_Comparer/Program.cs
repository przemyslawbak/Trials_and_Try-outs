using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static void PrintMessage(string message)
        {
            dynamic json = JObject.Parse(message);
            if (json.type == "q")
            {

                Console.WriteLine("Message type: Quote");
                Console.WriteLine("\tInstrument symbol: " + json.s);
                Console.WriteLine("\tTime: " + json.tm);
                if (json["so"] != null) Console.WriteLine("\tSource data feed: " + json.so);
                if (json["f"] != null) Console.WriteLine("\tQuote flags: " + json.f);
                if (json["co"] != null) Console.WriteLine("\tQuote condition: " + json.co);
                if (json["b"] != null) Console.WriteLine("\tBid price: " + json.b);
                if (json["bsz"] != null) Console.WriteLine("\tBid size: " + json.bsz);
                if (json["bex"] != null) Console.WriteLine("\tBid exchange: " + json.bex);
                if (json["a"] != null) Console.WriteLine("\tAsk price: " + json.a);
                if (json["asz"] != null) Console.WriteLine("\tAsk size: " + json.asz);
                if (json["aex"] != null) Console.WriteLine("\tAsk exchange: " + json.aex);
            }
            else if (json.type == "t")
            {
                Console.WriteLine("Message type: Trade");
                Console.WriteLine("\tInstrument symbol: " + json.s);
                Console.WriteLine("\tTime: " + json.tm);
                if (json["so"] != null) Console.WriteLine("\tSource data feed: " + json.so);
                if (json["f"] != null) Console.WriteLine("\tTrade flags: " + json.f);
                if (json["co"] != null) Console.WriteLine("\tTrade conditions: " + string.Join(", ", json.co));
                if (json["l"] != null) Console.WriteLine("\tLast price: " + json.l);
                if (json["sz"] != null) Console.WriteLine("\tLast size: " + json.sz);
                if (json["ex"] != null) Console.WriteLine("\tLast exchange: " + json.ex);
            }
        }

        // This is the MainAsync method that starts the program.
        static async Task MainAsync()
        {
            // Create an instance of HttpClient.
            var client = new HttpClient();
            // Streaming connections require header "Connection" "Keep-Alive".
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

            var baseUrl = "https://api.activetick.com/stream.json";
            var queryParams = new Dictionary<string, string>
                {
                    { "sessionid", "acf9d3ce3e944261a27d5762f693a070" },
                    { "symbols", "SPY_S U,AAPL_S U,EUR/USD_CFI,ES_230300_FCU" }
                };

            // Join the query parameters into a query string.
            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var url = $"{baseUrl}?{queryString}";

            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();

            var buffer = new byte[4096];
            var incompleteMessage = string.Empty;

            // Read data from the stream continuously.
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                // If the stream has no more data to read, break the loop.
                if (bytesRead == 0)
                    break;

                var chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // Split the chunk into complete JSON messages and any remaining bytes.
                var result = SplitJSONMessages(incompleteMessage + chunk);

                // Loop through each complete message and print it.
                foreach (var message in result.Messages)
                {
                    if (message.Length > 0)
                        PrintMessage(message);
                }

                // Store any remaining bytes for the next read.
                incompleteMessage = result.RemainingBytes;
            }
        }


        // SplitJSONMessages method takes in a chunk of string as input
        // and returns a tuple of string array and string
        static (string[] Messages, string RemainingBytes) SplitJSONMessages(string chunk)
        {
            var messages = new List<string>();
            var currentMessage = string.Empty;
            var openBrackets = 0;

            // Loop through each character in the chunk
            foreach (var c in chunk)
            {
                currentMessage += c;
                if (c == '{')
                    openBrackets++;
                else if (c == '}')
                    openBrackets--;
                // If the open brackets counter reaches 0, it means a complete JSON object has been found
                if (openBrackets == 0)
                {
                    // Add the complete JSON object to the list of messages
                    messages.Add(currentMessage.Trim());
                    currentMessage = string.Empty;
                }
            }

            // Return the list of messages as an array and the remaining bytes as a string
            return (messages.ToArray(), currentMessage);
        }
    }
}
