using System.Net.Http;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        private static HttpClient _client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/120.0");
            int spread = 199980;
            UrlVault urlVault = new UrlVault();
            string itemSearched = "DXY";
            var to = urlVault.GetToMaxInt();
            var from = to - spread;
            var dt = urlVault.GetToMaxIntUtcTimeStampe();
            var min = urlVault.GetToMinInt();

            while (from > min)
            {
                string url = urlVault.GetBaseUrl() + urlVault.GetResolution() + "&from=" + from + "&to=" + to;
                var response = await _client.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync(); //no data loaded :(
            }
        }
    }
}
