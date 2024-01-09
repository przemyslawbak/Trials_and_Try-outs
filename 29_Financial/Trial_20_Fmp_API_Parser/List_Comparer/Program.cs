using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        private static HelperService _helper = new HelperService();
        private static UrlVault _url = new UrlVault();
        private static HttpClient _client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/120.0");

            string[] companySymbols = _helper.GetSymbols().Distinct().ToArray();

            var y = 0;
            foreach (var symbol in companySymbols)
            {
                y++;
                await GetAndSaveCompanyTicks(symbol, y);
            }

        }

        private static async Task GetAndSaveCompanyTicks(string symbol, int y)
        {
            var companyTicks = new List<OhlcvObject>();
            int[] years = new int[] { 2024, 2023 };
            int[] months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] days = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

            for (int q = years.Length - 1; q >= 0; q--)
            {
                for (int i = months.Length - 1; i >= 0; i--)
                {
                    for (int j = days.Length - 1; j >= 0; j--)
                    {
                        var url = _url.GetFirstPart() + symbol + _url.SecondPart() + years[q] + "-" + months[i].ToString("D2") + "-" + days[j].ToString("D2") + _url.ThirdPart() + years[q] + "-" + months[i].ToString("D2") + "-" + days[j].ToString("D2") + _url.FourthPart();
                        var response = await _client.GetAsync(url);
                        var json = await response.Content.ReadAsStringAsync();
                        if (!json.Contains(DateTime.Now.ToShortDateString()))
                        {
                            Console.Clear();
                            Console.WriteLine(y.ToString("D3") + ". Processing " + symbol + " " + years[q] + "-" + months[i].ToString("D2") + "-" + days[j].ToString("D2"));

                            var dataFromTheDay = JsonConvert.DeserializeObject<IEnumerable<OhlcvObject>>(json);
                            dataFromTheDay = dataFromTheDay
                                .Select(x => new OhlcvObject()
                                {
                                    Open = x.Open,
                                    High = x.High,
                                    Low = x.Low,
                                    Close = x.Close,
                                    Volume = x.Volume,
                                    Capital = x.Close * x.Volume,
                                    Symbol = symbol,
                                    EstTimeStamp = x.EstTimeStamp,
                                    UtcTimeStamp = ConvertToUtc(x.EstTimeStamp),
                                });
                            companyTicks.AddRange(dataFromTheDay);
                        }
                    }
                }
            }

            var toSave = companyTicks.GroupBy(g => g.UtcTimeStamp).Select(c => c.OrderByDescending(w => w.UtcTimeStamp).First()).ToList();
            File
                .WriteAllLines(symbol + ".txt", toSave
                .Select(x => 
                x.UtcTimeStamp + "|" + 
                x.Open + "|" + 
                x.High + "|" + 
                x.Low + "|" + 
                x.Close + "|" +
                string.Format("{0}", (int)x.Volume) + "|" + 
                x.Capital
                ));
        }

        private static DateTime ConvertToUtc(DateTime estTimeStamp)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(estTimeStamp, est);
        }
    }
}
