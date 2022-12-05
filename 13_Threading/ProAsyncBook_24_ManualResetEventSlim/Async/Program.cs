using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 4-20. The BufferPool Implementation
    class Program
    {
        public static object MatchesFound { get; private set; }
        static ManualResetEventSlim controlFileAvailable = new ManualResetEventSlim();

        static void Main(string[] args)
        {
            var matchers = new[] { "dowjones", "ftse", "nasdaq", "dax" };
            var tasks = new List<Task>();

            foreach (string matcherName in matchers)
            {
                var matcher = new Matcher(matcherName, MatchesFound, controlFileAvailable);
                tasks.Add(matcher.Process());
            }
            Console.WriteLine("Press enter when control file ready");
            Console.ReadLine();
            controlFileAvailable.Set();
            Task.WaitAll(tasks.ToArray());
        }

        private void InternalProcess()
        {
            IEnumerable<TradeDay> days = Initialize();
            controlFileAvailable.Wait();
            ControlParameters parameters = GetControlParameters();
            IEnumerable<TradeDay> matchingDays = null;
            if (parameters != null)
            {
                matchingDays = from d in days
                               where d.Date >= parameters.FromDate &&
                               d.Date <= parameters.ToDate && d.Volume >= parameters.Volume
                               select d;
            }
            matchesFound(dataSource, matchingDays);
        }
    }
}
