using Newtonsoft.Json;
using System;
using System.Collections;
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
            /*List<string> weekRes = new List<string>();
            var weeksBack = 100;

            for (int i = 0; i < weeksBack; i++)
            {
                Console.WriteLine(i + " of " + weeksBack);
                DateTime utcNowTimestamp = DateTime.UtcNow.AddDays(-i * 7).AddDays(-5);
                DateTime utcMonthBackTimestamp = DateTime.UtcNow.AddDays(-i * 7).AddDays(-60).AddDays(-5);
                string apiKey = _keyLocker.GetApiKey();
                string indexName = "SPX";
                var calendarUrl = _service.GetCalendarUrl(apiKey, utcNowTimestamp, utcMonthBackTimestamp);
                var interval = Interval.Minute;

                var socialTradingValue = await TriggerParallelSocialCollectAndComputeAsync(calendarUrl);
                weekRes.Add(socialTradingValue.ToString("0.00"));
            };

            System.IO.File.WriteAllLines("weekRes.txt", weekRes);*/

            DateTime utcNowTimestamp = DateTime.UtcNow.AddMonths(1);
            DateTime utcMonthBackTimestamp = DateTime.UtcNow.AddMonths(-1);
            string apiKey = _keyLocker.GetApiKey();
            string indexName = "SPX";
            var calendarUrl = _service.GetCalendarUrl(apiKey, utcNowTimestamp, utcMonthBackTimestamp);
            var interval = Interval.Minute;

            var socialTradingValue = await TriggerParallelSocialCollectAndComputeAsync(calendarUrl); //3.54

            Console.ReadLine();
        }

        private static async Task<decimal> TriggerParallelSocialCollectAndComputeAsync(string dataUrl)
        {
            var json = await _scrapper.GetHtml(dataUrl);
            var data = JsonConvert.DeserializeObject<List<CalendarObject>>(json);
            var eventWeights = _service.GetEventWeights();
            var countryWeights = _service.GetCountryWeights();
            var impactWeights = _service.GetImpactWeights();
            var items = data;

            var result = ComputeCalendarItemsValue(items, eventWeights, countryWeights, impactWeights);

            return result;
        }

        private static decimal ComputeCalendarItemsValue(List<CalendarObject> items, Dictionary<string, int> eventWeights, Dictionary<string, decimal> countryWeights, Dictionary<string, decimal> impactWeights)
        {
            List<decimal> calendarResults = new List<decimal>();

            foreach (var thing in items)
            {
                if (thing.Event.Contains("("))
                {
                    thing.Event = thing.Event.Split('(')[0].Trim();
                }

                try
                {
                    decimal diffPrevious = 0;
                    decimal diffEstimate = 0;
                    decimal diffPrognosis = 0;
                    var weightCountry = countryWeights[thing.CountryCode];
                    var weightImpact = impactWeights[thing.Impact];
                    var weightEvent = eventWeights.Where(x => thing.Event.Contains(x.Key)).Select(x => x.Value).First();

                    decimal weight = weightCountry * weightImpact * weightEvent;

                    if (thing.Actual.HasValue && thing.Previous.HasValue)
                    {
                        diffPrevious = thing.Actual.Value - thing.Previous.Value < 0 ? -1 : 1;
                    }
                    if (thing.Actual.HasValue && thing.Estimate.HasValue)
                    {
                        diffEstimate = thing.Actual.Value - thing.Estimate.Value < 0 ? -1 : 1;
                    }
                    if (!thing.Actual.HasValue && thing.Estimate.HasValue && thing.Previous.HasValue)
                    {
                        diffPrognosis = thing.Estimate.Value - thing.Previous.Value < 0 ? -1 : 1;
                    }

                    calendarResults.Add((diffPrevious + diffEstimate * 0.5M + diffPrognosis) * weight);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            return calendarResults.Sum(x => x);
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
