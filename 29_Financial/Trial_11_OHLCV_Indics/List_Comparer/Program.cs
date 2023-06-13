﻿using Newtonsoft.Json;
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

            var hgppValues = await TriggerParallelOhlcvCollectAndComputeAsync(indexName, ohlcvUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.ReadLine();
        }

        private static async Task<OhlcvTaResult> TriggerParallelOhlcvCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            List<OhlcvTaResult> results = new List<OhlcvTaResult>();
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
                        var multiplier = companyList.Where(x => x.Code == method).Select(x => x.Weight).First();

                        List<OhlcvObject> items = data.Select((x, index) => new OhlcvObject() 
                        { 
                            Capital = x.Volume * x.Close,
                            Symbol = method,
                            Multiplier = multiplier,
                            Close = x.Close,
                            High = x.High,
                            Low = x.Low,
                            Open = x.Open,
                            Volume = x.Volume,
                            Index = index
                        }).ToList();

                        OhlcvTaResult result = new OhlcvTaResult()
                        {
                            AwesomeOscillator = GetAwesomeOscillatorValue(items),
                            ChaikinMoneyFlow = GetChaikinMoneyFlowValue(items),
                            ElderRay = GetElderRayValue(items),
                            MoneyFlowIndex = GetMoneyFlowIndexValue(items),
                            ShaffTrendCycle = GetShaffTrendCycleValue(items),
                        };

                        results.Add(result);

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

            var sumAwesomeOscillator = results.Sum(x => x.AwesomeOscillator);
            var sumAhaikinMoneyFlow = results.Sum(x => x.ChaikinMoneyFlow);
            var sumElderRay = results.Sum(x => x.ElderRay);
            var sumMoneyFlowIndex = results.Sum(x => x.MoneyFlowIndex);
            var sumShaffTrendCycle = results.Sum(x => x.ShaffTrendCycle);

            return new OhlcvTaResult() 
            {
                AwesomeOscillator = sumAwesomeOscillator,
                ChaikinMoneyFlow = sumAhaikinMoneyFlow,
                ElderRay = sumElderRay,
                MoneyFlowIndex = sumMoneyFlowIndex,
                ShaffTrendCycle = sumShaffTrendCycle,
            };
        }

        private static decimal GetShaffTrendCycleValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetMoneyFlowIndexValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetElderRayValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetChaikinMoneyFlowValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }

        private static decimal GetAwesomeOscillatorValue(List<OhlcvObject> items)
        {
            throw new NotImplementedException();
        }
    }
}
