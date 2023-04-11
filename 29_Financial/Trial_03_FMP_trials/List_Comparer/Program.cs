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
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://financialmodelingprep.com/api/v3/search?query=KGH&limit=100&apikey=" + k))

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
