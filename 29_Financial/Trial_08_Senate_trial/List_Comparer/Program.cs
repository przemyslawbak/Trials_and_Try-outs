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
            var senateUrlDictionary = _service.GetPtcUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var senateTradingValue = await TriggerParallelSenateCollectAndComputeAsync(indexName, senateUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.ReadLine();
        }

        private static async Task<decimal> TriggerParallelSenateCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
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
                    var amountDict = GetAmountDictionary();

                    try
                    {
                        var json = await _scrapper.GetHtml(url);
                        var data = JsonConvert.DeserializeObject<List<SenateTradingObject>>(json);
                        var item = data;
                        item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();

                        results.Add(ComputeSenateTradingItemsValue(item, amountDict));
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

            return results.Sum(x => Convert.ToDecimal(x));
        }

        private static Dictionary<string, int> GetAmountDictionary()
        {
            return new Dictionary<string, int>()
            {
                { "$15,001 - $50,000", 50000 },
                { "$1,001 - $15,000", 15000 },
                { "$50,001 - $100,000", 100000 },
                { "$100,001 - $250,000", 250000 },
                { "$250,001 - $500,000", 500000 },
            };
        }

        private static decimal ComputeSenateTradingItemsValue(object item, Dictionary<string, int> amountDict)
        {
            var ptcBase = (List<SenateTradingObject>)item;

            var amounts = new List<decimal>();

            foreach (var fing in ptcBase)
            {
                if (fing.TransactionDate > DateTime.Now.AddDays(-55))
                {
                    var transactionMultiplier = fing.Type.Contains("Purchase") ? 1 : -1;
                    var amountValue = amountDict[fing.Amount];
                    var multi = ptcBase[0].Multiplier;

                    decimal res = (decimal)transactionMultiplier * amountValue * multi;

                    amounts.Add(res);
                }
            }

            return amounts.Sum(x => x);
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
