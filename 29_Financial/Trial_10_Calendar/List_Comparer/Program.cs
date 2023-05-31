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
            DateTime utcMonthBackTimestamp = DateTime.UtcNow.AddMonths(-1);
            string apiKey = _keyLocker.GetApiKey();
            string indexName = "SPX";
            var calendarUrl = _service.GetCalendarUrl(apiKey, utcNowTimestamp, utcMonthBackTimestamp);
            var interval = Interval.Minute;

            var socialTradingValue = await TriggerParallelSocialCollectAndComputeAsync(calendarUrl);

            Console.ReadLine();
        }

        private static async Task<decimal> TriggerParallelSocialCollectAndComputeAsync(string dataUrl)
        {
            var json = await _scrapper.GetHtml(dataUrl);
            var data = JsonConvert.DeserializeObject<List<CalendarObject>>(json);
            var eventWeights = _service.GetEventWeights();
            var countryWeights = _service.GetCountryWeights();
            var item = data;

            var result = ComputeCalendarItemsValue(item, eventWeights, countryWeights);

            return result;
        }

        private static decimal ComputeCalendarItemsValue(List<CalendarObject> item, Dictionary<string, decimal> eventWeights, Dictionary<string, decimal> countryWeights)
        {
            return 0;
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
