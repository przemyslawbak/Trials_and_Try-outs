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
            Task.Run(async () => await DoSomething());
            Waiter().Wait();
        }

        private static async Task Waiter()
        {
            while (true)
            {
                await Task.Delay(1000);
            }
        }

        private static async Task DoSomething()
        {
            var history = true; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            DateTime utcNowTimestamp = DateTime.UtcNow;
            string apiKey = _keyLocker.GetApiKey();
            var companyList = _service.GetIndexComponents().Select(x => new CompanyModel() { Code = x.Key, Weight = x.Value, UtcTimeStamp = utcNowTimestamp }).ToList();
            string indexName = "SPX";
            var socialUrlDictionary = _service.GetSocialUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var socialTradingValue = await TriggerParallelSocialCollectAndComputeAsync(indexName, socialUrlDictionary, utcNowTimestamp, interval, companyList, history);

            Console.WriteLine("Social: " + socialTradingValue);
        }

        private static async Task<decimal> TriggerParallelSocialCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList, bool history)
        {
            List<decimal> results = new List<decimal>();
            var exceptions = 0;
            var positive = 0;

            for (int i = 0; i < dataUrls.Count; i++)
            {
                string method = dataUrls.ElementAt(i).Key;
                string url = dataUrls.ElementAt(i).Value;
                await Task.Delay(10);
                int iteration = i;

                try
                {
                    if (history)
                    {
                        var companyHistoryResults = new List<SocialObjectHistory>();
                        var items = new List<SocialObjectHistory>();
                        var historyPages = 3;
                        //var historyPages = 100;

                        for (int iter = 0; iter <= historyPages; iter++)
                        {
                            Console.WriteLine(dataUrls.ElementAt(i).Key + " page: " + iter);
                            var json = await _scrapper.GetHtml(url.Replace("page=0", "page=" + iter));
                            var data = JsonConvert.DeserializeObject<List<SocialObjectHistory>>(json);
                            var item = data;
                            if (data.Count > 0)
                            {
                                item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();
                                items.AddRange(item);
                                companyHistoryResults.AddRange(items);

                                //todo: how many items in 'page'? 10?

                                /*foreach (var xxx in item)
                                {
                                    companyHistoryResults.Add(ComputeSocialItemsValueHistory(xxx, item[0].Multiplier));
                                }*/
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (items.Count > 0)
                        {
                            positive++;
                        }

                        Console.WriteLine("res: " + companyHistoryResults.Count);
                        Console.WriteLine("Computing result values...");

                        var modifiedResults = companyHistoryResults.Select((x, ind) => new SocialObjectHistory()
                        {
                            Comments= x.Comments,
                            EstTimeStamp = x.EstTimeStamp,
                            DataSet= GetDataSet(companyHistoryResults, ind),
                            Impressions = x.Impressions,
                            Likes= x.Likes,
                            Multiplier = x.Multiplier,
                            Posts= x.Posts,
                            ResultValue= x.ResultValue,
                            Sentiment= x.Sentiment,
                            Symbol = x.Symbol,
                        }).ToList();


                        modifiedResults = companyHistoryResults.Select((x, ind) => new SocialObjectHistory()
                        {
                            Comments = x.Comments,
                            EstTimeStamp = x.EstTimeStamp,
                            DataSet = x.DataSet,
                            Impressions = x.Impressions,
                            Likes = x.Likes,
                            Multiplier = x.Multiplier,
                            Posts = x.Posts,
                            ResultValue = GetResult(x.DataSet),
                            Sentiment = x.Sentiment,
                            Symbol = x.Symbol,
                        }).ToList();

                        //todo: save companyHistoryResults

                    }
                    else
                    {
                        var json = await _scrapper.GetHtml(url);
                        var data = JsonConvert.DeserializeObject<List<SocialObject>>(json);
                        var item = data;
                        item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();

                        results.Add(ComputeSocialItemsValue(item));
                    }
                }
                catch (Exception ex)
                {
                    exceptions++;
                }
            }

            Console.WriteLine("Exceptions: " + exceptions);

            var aver = results.Average();

            return results.Sum(x => Convert.ToDecimal(x)) / 100000;
        }

        private static decimal GetResult(List<SocialObjectHistory> item)
        {
            var itemsList = (List<SocialObjectHistory>)item;
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

        private static List<SocialObjectHistory> GetDataSet(List<SocialObjectHistory> companyHistoryResults, int ind)
        {
            var pageItemsQty = 250;

            return companyHistoryResults.GetRange(ind, ind + pageItemsQty);
        }

        /*private static async Task<decimal> TriggerParallelSocialCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList, bool history)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            List<decimal> results = new List<decimal>();
            var exceptions = 0;
            var positive = 0;

            for (int i = 0; i < dataUrls.Count; i++)
            {
                await Task.Delay(10);
                int iteration = i;

                currentRunningTasks.Add(Task.Run(async () =>
                {
                    string method = dataUrls.ElementAt(iteration).Key;
                    string url = dataUrls.ElementAt(iteration).Value;

                    try
                    {
                        if (history)
                        {
                            var companyHistoryResults = new List<SocialObjectHistory>();
                            var items = new List<SocialObjectHistory>();
                            var historyPages = 100;

                            for (int iter = 0; iter <= historyPages; i++)
                            {
                                var json = await _scrapper.GetHtml(url.Replace("page=0", "page=" + i));
                                var data = JsonConvert.DeserializeObject<List<SocialObjectHistory>>(json);
                                var item = data;
                                if (data.Count > 0)
                                {
                                    item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();
                                    items.AddRange(item);

                                    //todo: how many items in 'page'? 10?

                                    *//*foreach (var xxx in item)
                                    {
                                        companyHistoryResults.Add(ComputeSocialItemsValueHistory(xxx, item[0].Multiplier));
                                    }*//*
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (items.Count > 0)
                            {
                                positive++;
                            }

                            //todo: save companyHistoryResults

                        }
                        else
                        {
                            var json = await _scrapper.GetHtml(url);
                            var data = JsonConvert.DeserializeObject<List<SocialObject>>(json);
                            var item = data;
                            item[0].Multiplier = companyList.Where(x => x.Code == item[0].Symbol).Select(x => x.Weight).First();

                            results.Add(ComputeSocialItemsValue(item));
                        }



                    }
                    catch (Exception ex)
                    {
                        exceptions++;
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(30000));
            tokenSource.Cancel();

            Console.WriteLine("Exceptions: " + exceptions); //why does it fisnih so quick?

            var aver = results.Average();

            return results.Sum(x => Convert.ToDecimal(x)) / 100000;
        }*/

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
