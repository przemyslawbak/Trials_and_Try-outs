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
            var ptcUrlDictionary = _service.GetPtcUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;
            var stockShortPricesUrl = _service.GetStockPricesUrl(companyList, apiKey);

            var ptcValue = await TriggerParallelPtcCollectAndComputeAsync(indexName, ptcUrlDictionary, utcNowTimestamp, interval, companyList, stockShortPricesUrl);

            Console.ReadLine();
        }

        private static async Task<decimal> TriggerParallelPtcCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList, string stockShortPricesUrl)
        {
            var stockShortPrices = await GetPrices(stockShortPricesUrl);

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
                        var data = JsonConvert.DeserializeObject<List<PtcObject>>(json);
                        var item = data[0];
                        item.Multiplier = companyList.Where(x => x.Code == item.Symbol).Select(x => x.Weight).First();
                        item.Price = stockShortPrices.Where(x => x.Key == item.Symbol).Select(x => x.Value).First();

                        results.Add(ComputePtcItemsValue(item));
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(30000));
            tokenSource.Cancel();

            var aver = results.Average();

            return results.Sum(x => Convert.ToDecimal(x)) / 100;
        }

        private static async Task<Dictionary<string, decimal>> GetPrices(string url)
        {
            Dictionary<string, decimal> res = new Dictionary<string, decimal>();

            try
            {
                var json = await _scrapper.GetHtml(url);
                var data = JsonConvert.DeserializeObject<List<PriceObject>>(json);
                res = data.Select(x => new{ x.Symbol, x.Price }).ToDictionary(x => x.Symbol, x => x.Price);
            }
            catch (Exception ex)
            {
                //do nothing
            }

            return res;
        }

        private static decimal ComputePtcItemsValue(object item)
        {
            var ptcBase = (PtcObject)item;

            var perc = (ptcBase.TargetConsensus - ptcBase.Price) / ptcBase.Price * ptcBase.Multiplier;
            return perc;
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
