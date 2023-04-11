using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace List_Comparer
{
    //docs: https://site.financialmodelingprep.com/developer/docs/
    class Program
    {
        private static Hidden _hiddenService;
        static void Main(string[] args)
        {
            _hiddenService = new Hidden();
            DoSomething().Wait();
        }

        private static async Task DoSomething()
        {
            string k = _hiddenService.GetK();

            //Stock Ticker search lookup API
            using (var httpClient = new HttpClient())
            {
                string symbol = "AAPL";
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/search?query=" + symbol + "&limit=100&apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(d);
                }
            }

            //Stock Historical Price
            using (var httpClient = new HttpClient())
            {
                string symbol = "AAPL";
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/historical-chart/1min/" + symbol + "?apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(d);
                }
            }

            //Index Historical Price
            using (var httpClient = new HttpClient())
            {
                //SP500
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/historical-chart/1min/%5EGSPC?apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(d);
                }
            }

            //Market Index
            using (var httpClient = new HttpClient())
            {
                //SP500
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/quote/%5EGSPC?apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    //onsole.WriteLine(d);
                }
            }

            //SP500 companies
            using (var httpClient = new HttpClient())
            {
                //SP500, no weight :(
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/sp500_constituent?apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(d);
                }
            }

            //Historical SP500 components
            using (var httpClient = new HttpClient())
            {
                //SP500
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/historical/sp500_constituent?apikey=" + k))

                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                    var response = await httpClient.SendAsync(request);

                    var d = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(d);
                }
            }

            Console.ReadKey();
        }
    }
}
