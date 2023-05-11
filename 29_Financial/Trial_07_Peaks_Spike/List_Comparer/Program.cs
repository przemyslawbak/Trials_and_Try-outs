using Newtonsoft.Json;
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
            string indexName = "SPX";
            var ohlcvUrlDictionary = _service.GetStock1minUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var peakValue = await TriggerParallelPeaksCollectAndComputeAsync(indexName, ohlcvUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.ReadLine();
        }

        private static async Task<decimal> TriggerParallelPeaksCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            List<decimal> results = new List<decimal>();

            for (int i = 0; i < dataUrls.Count; i++)
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
                        var multiplier = companyList.Where(x => x.Code == method).Select(x => x.Weight).First();

                        List<OhlcvObject> items = data.Select(x => new OhlcvObject() 
                        { 
                            Capital = x.Volume * x.Close,
                            Symbol = method,
                            Multiplier = multiplier,
                            Close = x.Close,
                            High = x.High,
                            Low = x.Low,
                            Open = x.Open,
                            Volume = x.Volume
                        }).ToList();

                        var peakValue = GetPeakValue(items);
                        var gapsValue = GetGapsValue(items);
                        var hammersValue = GetHammersValue(items);

                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(30000));
            tokenSource.Cancel();

            return results.Sum(x => Convert.ToDecimal(x)) / 100;
        }

        private static decimal GetHammersValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetGapsValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetPeakValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
