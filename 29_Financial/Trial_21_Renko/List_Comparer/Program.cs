using Newtonsoft.Json;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        private static HelperService _service = new HelperService();
        private static KeyLocker _keyLocker = new KeyLocker();
        private static Scrapper _scrapper = new Scrapper();
        static void Main(string[] args)
        {
            DoSomething().Wait();
        }

        private static async Task DoSomething()
        {
            DateTime utcNowTimestamp = DateTime.UtcNow;
            string apiKey = _keyLocker.GetApiKey();
            var companyList = _service.GetIndexComponents().Select(x => new CompanyModel() { Code = x.Key, Weight = x.Value, UtcTimeStamp = utcNowTimestamp }).ToList();
            companyList = companyList.OrderByDescending(x => x.Weight).ToList();
            string indexName = "SPX";
            var ohlcvUrlDictionary = _service.GetStock1minUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var indValues = await TriggerParallelOhlcvCollectAndComputeAsync(indexName, ohlcvUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.WriteLine(indValues);
            Console.WriteLine();
        }

        private static async Task<OhlcvTaResult> TriggerParallelOhlcvCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            var exceptions = 0;

            for (int i = 0; i < 100; i++) //limited to 100 (70% of index weight)
            {
                int iteration = i;

                currentRunningTasks.Add(Task.Run(async () =>
                {
                    string method = dataUrls.ElementAt(iteration).Key;
                    string url = dataUrls.ElementAt(iteration).Value;

                    try
                    {
                        var json = await _scrapper.GetHtml(url);
                        var data = JsonConvert.DeserializeObject<List<OhlcvObject>>(json);
                        data.Reverse();

                        var quotes = data.Select(x => new Quote()
                        {
                            Close = x.Close,
                            Date = x.Date,
                            High = x.High,
                            Low = x.Low,
                            Open = x.Open,
                            Volume = x.Volume,
                        }).ToList();

                        var renko = quotes.GetRenkoAtr(100, EndType.Close).ToList();

                        var res = renko.Last().IsUp ? 1 : -1; //<------
                    }
                    catch (Exception ex)
                    {
                        exceptions++;
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(120000));
            tokenSource.Cancel();

            Console.WriteLine("Exceptions: " + exceptions);

            return new OhlcvTaResult();
        }

        

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
