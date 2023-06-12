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
            var socialUrlDictionary = _service.GetSocialUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var socialTradingValue = await TriggerParallelSocialCollectAndComputeAsync(indexName, socialUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.WriteLine("Social: " + socialTradingValue);
        }

        private static async Task<decimal> TriggerParallelSocialCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
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
                        var data = JsonConvert.DeserializeObject<List<SocialObject>>(json);
                        var item = data;
                        item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();

                        results.Add(ComputeSocialItemsValue(item));
                    }
                    catch (Exception ex)
                    {
                        exceptions++;
                        Console.WriteLine(ex.Message);
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(30000));
            tokenSource.Cancel();

            Console.WriteLine("Exceptions: " + exceptions);

            var aver = results.Average();

            return results.Sum(x => Convert.ToDecimal(x)) / 100000;
        }

        private static decimal ComputeSocialItemsValue(object item)
        {
            var itemsList = (List<SocialObject>)item;
            List<decimal> socialList = new List<decimal>();
            var multi = itemsList[0].Multiplier;

            foreach (var fing in itemsList)
            {
                var added = (decimal)fing.Likes * 0.1M + (decimal)fing.Comments * 1 + (decimal)fing.Posts * 1 + (decimal)fing.Impressions * 0.00001M;
                decimal res = (decimal)added * (decimal)fing.Sentiment * multi;

                socialList.Add(res);
            }

            return socialList.Sum(x => x);
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
