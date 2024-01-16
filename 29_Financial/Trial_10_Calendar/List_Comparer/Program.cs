using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

                var socialTradingValue = await TriggerParallelCalendarCollectAndComputeAsync(calendarUrl);
                weekRes.Add(socialTradingValue.ToString("0.00"));
            };

            System.IO.File.WriteAllLines("weekRes.txt", weekRes);*/

            string indexName = "SPX";
            var interval = Interval.Minute;

            var calendarValues = await TriggerParallelCalendarCollectAndComputeAsync();

            Console.ReadLine();
        }/*

        private static async Task<decimal> TriggerParallelCalendarCollectAndComputeAsync(string dataUrl)
        {
            var json = await _scrapper.GetHtml(dataUrl);
            var data = JsonConvert.DeserializeObject<List<CalendarObject>>(json);
            var countries = data.GroupBy(x => x.Event).Select(x => x.Key).ToList();
            System.IO.File.WriteAllLines("_events_codes.txt", countries);
            var eventWeights = _service.GetEventWeights();
            var countryWeights = _service.GetCountryWeights();
            var impactWeights = _service.GetImpactWeights();
            var items = data;

            var result = ComputeCalendarItemsValue(items, eventWeights, countryWeights, impactWeights);

            return result;
        }*/

        private static async Task<decimal> TriggerParallelCalendarCollectAndComputeAsync()
        {
            List<CalendarObjectHistory> objects = new List<CalendarObjectHistory>();
            DateTime utcNowTimestamp = DateTime.UtcNow.AddMonths(1);
            DateTime utcMonthBackTimestamp = DateTime.UtcNow.AddMonths(-1);
            string apiKey = _keyLocker.GetApiKey();

            string[] dateRanges = new string[]
            {
                "2024-01-01&to=2024-01-31",
                "2023-12-01&to=2023-12-31",
                "2023-11-01&to=2023-11-31",
                "2023-10-01&to=2023-10-31",
                "2023-09-01&to=2023-09-31",
                "2023-08-01&to=2023-08-31",
                "2023-07-01&to=2023-07-31",
                "2023-06-01&to=2023-06-31",
                "2023-05-01&to=2023-05-31",
                "2023-04-01&to=2023-04-31",
                "2023-03-01&to=2023-03-31",
                "2023-02-01&to=2023-02-31",
                "2023-01-01&to=2023-01-31",
                "2022-12-01&to=2022-12-31",
            };
            var eventWeights = _service.GetEventWeights();
            var countryWeights = _service.GetCountryWeights();
            var impactWeights = _service.GetImpactWeights();

            foreach (var range in dateRanges)
            {
                Console.WriteLine(range);
                var calendarUrl = _service.GetCalendarUrl(apiKey, utcNowTimestamp, utcMonthBackTimestamp, range);

                var json = await _scrapper.GetHtml(calendarUrl);
                List<CalendarObjectHistory> data = JsonConvert.DeserializeObject<List<CalendarObjectHistory>>(json);

                objects.AddRange(data);
                Console.WriteLine("res: " + data.Count);

                //var countries = data.GroupBy(x => x.CountryCode).Select(x => x.Key).ToList();
                //System.IO.File.WriteAllLines("country_codes.txt", countries);

                //var result = ComputeCalendarItemsValue(data, eventWeights, countryWeights, impactWeights);
            }

            Console.WriteLine("Computing result values...");

            var modifiedResults = objects.Select((x, ind) => new CalendarObjectHistory()
            {
                EstTimeStamp = x.EstTimeStamp,
                DataSet = GetDataSet(objects, ind),
                ResultValue = x.ResultValue,
                UtcTimeStamp = ConvertToUtc(x.EstTimeStamp),
                Actual = x.Actual,
                CountryCode = x.CountryCode,
                Estimate = x.Estimate,
                Event = x.Event,
                Impact = x.Impact,
                Previous = x.Previous
            }).ToList();

            var toBeSaved = modifiedResults.Select(x => new CalendarObjectHistory()
            {
                EstTimeStamp = x.EstTimeStamp,
                DataSet = x.DataSet,
                ResultValue = GetResult(x.DataSet, eventWeights, countryWeights, impactWeights),
                UtcTimeStamp = x.UtcTimeStamp,
                Actual = x.Actual,
                CountryCode = x.CountryCode,
                Estimate = x.Estimate,
                Event = x.Event,
                Impact = x.Impact,
                Previous = x.Previous
            }).ToList();

            var toSave = toBeSaved.GroupBy(g => g.UtcTimeStamp).Select(c => c.OrderByDescending(w => w.UtcTimeStamp).First()).ToList();
            File
                .WriteAllLines("_calendar.txt", toSave
                .Select(x =>
                x.UtcTimeStamp + "|" +
                x.ResultValue
                ));

            Console.WriteLine("Saved");

            return new decimal();
        }

        private static decimal GetResult(List<CalendarObjectHistory> dataSet, Dictionary<string, int> eventWeights, Dictionary<string, decimal> countryWeights, Dictionary<string, decimal> impactWeights)
        {
            if (dataSet != null)
            {
                List<decimal> calendarResults = new List<decimal>();

                foreach (var thing in dataSet)
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

            return 0.0M;
        }

        private static DateTime ConvertToUtc(DateTime estTimeStamp)
        {
            try
            {
                TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                return TimeZoneInfo.ConvertTimeToUtc(estTimeStamp, est);
            }
            catch (Exception ex)
            {

            }

            return DateTime.Now.AddYears(100);
        }

        private static List<CalendarObjectHistory> GetDataSet(List<CalendarObjectHistory> objects, int ind)
        {
            var ret = new List<CalendarObjectHistory>();
            var pageItemsQty = 0;
            try
            {
                pageItemsQty = 1200;

                if (pageItemsQty + ind > objects.Count - 1)
                {
                    pageItemsQty = objects.Count - 1;
                }

                ret = objects.GetRange(ind, pageItemsQty);
            }
            catch (Exception ex)
            {
                //do nothing
            }

            return ret;
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
