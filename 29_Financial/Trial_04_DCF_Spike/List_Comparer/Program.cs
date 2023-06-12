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
            var dcfUrlDictionary = _service.GetDcfUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var dcfValue = await TriggerParallelDcfCollectAndComputeAsync(indexName, dcfUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.WriteLine("DCF: " + dcfValue);
        }

        private static async Task<decimal> TriggerParallelDcfCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            List<decimal> results = new List<decimal>();
            var exceptions = 0;

            for (int i = 0; i < dataUrls.Count; i++)
            {
                await Task.Delay(1);
                int iteration = i;

                currentRunningTasks.Add(Task.Run(async () =>
                {
                    string method = dataUrls.ElementAt(iteration).Key;
                    string url = dataUrls.ElementAt(iteration).Value;

                    try
                    {
                        var json = await _scrapper.GetHtml(url);
                        var data = JsonConvert.DeserializeObject<List<DcfObject>>(json);
                        var item = data[0];
                        item.Multiplier = companyList.Where(x => x.Code == item.Symbol).Select(x => x.Weight).First();

                        results.Add(ComputeDcfItemsValue(item));
                    }
                    catch (Exception ex)
                    {
                        exceptions++;
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(3000000));
            tokenSource.Cancel();

            Console.WriteLine("Exceptions: " + exceptions);

            var aver = results.Average();

            return results.Sum(x => Convert.ToDecimal(x));
        }

        private static decimal ComputeDcfItemsValue(object item)
        {
            var dcfBase = (DcfObject)item;
            var perc = (dcfBase.Dcf - dcfBase.StockPrice) / dcfBase.StockPrice * dcfBase.Multiplier;
            return perc;
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
