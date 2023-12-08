using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            foreach (var symbol in companySymbols)
            {
                await GetAndSaveCompanyTicks(symbol);
            }

        }

        private static async Task GetAndSaveCompanyTicks(string symbol)
        {
            var companyTicks = new List<OhlcvObject>();
            int[] months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] days = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

            for (int i = months.Length - 1; i > 0; i--)
            {
                for (int j = days.Length - 1; j > 0; j--)
                {
                    var url = _url.GetFirstPart() + symbol + _url.SecondPart() + months[i].ToString("D2") + "-" + days[j].ToString("D2") + _url.ThirdPart() + months[i].ToString("D2") + "-" + days[j].ToString("D2") + _url.FourthPart();
                    var response = await _client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();
                    if (!json.Contains(DateTime.Now.ToShortDateString()))
                    {
                        Console.WriteLine("Processing " + symbol + " " + 2023 + "-" + months[i].ToString("D2") + "-" + days[j].ToString("D2"));

                        var dataFromTheDay = JsonConvert.DeserializeObject<List<OhlcvObject>>(json);
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
                            })
                            .OrderByDescending(x => x.EstTimeStamp)
                            .ToList();
                        companyTicks.AddRange(dataFromTheDay);
                    }
                }
            }

            //todo: save to the file
        }

        private static DateTime ConvertToUtc(DateTime estTimeStamp)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(estTimeStamp, est);
        }
    }
}
