using System;
using System.Globalization;
using System.IO;
using System.Linq;
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
            UrlVault urlVault = new UrlVault();
            string itemSearched = "USLIBOR1M";
            var dtReference = urlVault.GetToMaxIntUtcTimeStampe();
            var maxTo = urlVault.GetToMaxInt();

            //var maxTick = 1701941700;

            string url = urlVault.GetBaseUrl();
            var response = await _client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            var data = html
                .Split(new string[] { ":[[" }, StringSplitOptions.None)[1]
                .Split(new string[] { "]]}]" }, StringSplitOptions.None)[0];
            var data2 = data
                .Split(new string[] { "],[" }, StringSplitOptions.None)
                .Select(x => new DataObject()
                {
                    Tick = long.Parse(x.Split(',')[0]),
                    Value = decimal.Parse(x.Split(',')[1], System.Globalization.NumberStyles.AllowParentheses |
                 System.Globalization.NumberStyles.AllowLeadingWhite |
                 System.Globalization.NumberStyles.AllowTrailingWhite |
                 System.Globalization.NumberStyles.AllowThousands |
                 System.Globalization.NumberStyles.AllowDecimalPoint |
                 System.Globalization.NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture),
                })
                .Reverse()
                .ToList();

            for (int i = 0; i < data2.Count; i++)
            {
                var multiplier = (maxTo - data2[i].Tick) / 86400000;
                data2[i].UtcTimeStamp = dtReference.AddDays(-multiplier);
            }

            var toSave = data2.GroupBy(x => x.Tick).Select(z => z.OrderBy(y => y.UtcTimeStamp).First()).ToList();
            File.WriteAllLines("_" + itemSearched + ".txt", toSave.Select(x => x.UtcTimeStamp + "|" + x.Value + "|" + x.Tick));

            Console.WriteLine("Saved");
        }
    }
}
