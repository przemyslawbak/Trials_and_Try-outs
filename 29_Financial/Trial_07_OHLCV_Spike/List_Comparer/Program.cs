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
            companyList = companyList.OrderByDescending(x => x.Weight).ToList();
            string indexName = "SPX";
            var ohlcvUrlDictionary = _service.GetStock1minUrlDictionary(companyList, apiKey);
            var interval = Interval.Minute;

            var hgppValues = await TriggerParallelOhlcvCollectAndComputeAsync(indexName, ohlcvUrlDictionary, utcNowTimestamp, interval, companyList);

            Console.ReadLine();
        }

        private static async Task<OhlcvResult> TriggerParallelOhlcvCollectAndComputeAsync(string indexName, Dictionary<string, string> dataUrls, DateTime utcNowTimestamp, Interval interval, List<CompanyModel> companyList)
        {
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            List<OhlcvResult> results = new List<OhlcvResult>();

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

                        OhlcvResult result = new OhlcvResult()
                        {
                            Hammers = GetHammersValue(items), //* vol
                            Gaps = GetGapsValue(items), //*vol
                            Peaks = GetPeakSumValue(items), //*vol
                            Patterns = GetPatternsSumValue(items), //*vol
                            Momentum = GetMomentumSumValue(items),
                            Strength = GetStrengthSumValue(items),
                            Sentiment = GetSentimentSumValue(items), //*vol
                            McClellan = GetMcClellanSumValue(items),
                        };

                        results.Add(result);

                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }

                }, tokenSource.Token));
            }

            await Task.WhenAny(Task.WhenAll(currentRunningTasks), Task.Delay(120000));
            tokenSource.Cancel();

            var sumHamemrs = results.Sum(x => x.Hammers);
            var sumGaps = results.Sum(x => x.Gaps);
            var sumPeaks = results.Sum(x => x.Peaks);
            var sumPatterns = results.Sum(x => x.Patterns);
            var sumMomentum = results.Sum(x => x.Momentum);
            var sumStrength = results.Sum(x => x.Strength);
            var sumSentiment = results.Sum(x => x.Sentiment);
            var sumMcClellan = results.Sum(x => x.McClellan);

            return new OhlcvResult() 
            { 
                Hammers = sumHamemrs, 
                Gaps = sumGaps,
                Peaks = sumPeaks, 
                Patterns = sumPatterns, 
                Momentum = sumMomentum, 
                Strength = sumStrength,
                McClellan = sumMcClellan,
            };
        }

        private static decimal GetMcClellanSumValue(List<OhlcvObject> items)
        {
            var up19 = items.Skip(Math.Max(0, items.Count() - 19)).Where(x => x.Open < x.Close).Count();
            var down19 = items.Skip(Math.Max(0, items.Count() - 19)).Where(x => x.Open >= x.Close).Count();
            var up39 = items.Skip(Math.Max(0, items.Count() - 39)).Where(x => x.Open < x.Close).Count();
            var down39 = items.Skip(Math.Max(0, items.Count() - 39)).Where(x => x.Open >= x.Close).Count();

            var rana19 = (decimal)(up19 - down19) / (up19 + down19);
            var rana39 = (decimal)(up39 - down39) / (up39 + down39);

            return (rana19 - rana39) * items[0].Multiplier * 10;
        }

        private static decimal GetSentimentSumValue(List<OhlcvObject> items)
        {
            int sentimentPeriod = 14;
            var barHigh = GetSma(items.Select(x => x.High), sentimentPeriod);
            var barLow = GetSma(items.Select(x => x.Low), sentimentPeriod);
            var barClose = GetSma(items.Select(x => x.Close), sentimentPeriod);
            var barOpen = GetSma(items.Select(x => x.Open), sentimentPeriod);
            var barRange = barHigh - barLow;

            var groupHigh = items.Skip(Math.Max(0, items.Count() - sentimentPeriod)).Select(x => x.High).Max();
            var groupLow = items.Skip(Math.Max(0, items.Count() - sentimentPeriod)).Select(x => x.Low).Min();
            var groupOpen = items.Skip(Math.Max(0, items.Count() - sentimentPeriod + 1)).First().Open;
            var groupRange = groupHigh - groupLow;

            var barBull = (barClose - barLow) + (barHigh - barOpen) / 2;
            var barBear = (barHigh - barClose) + (barOpen - barLow) / 2;

            var volumeStrength = (decimal)items.Last().Volume / items.Sum(x => x.Volume) / items.Count;

            return (barBull - barBear) * items[0].Multiplier * volumeStrength * 100000;
        }

        private static decimal GetSma(IEnumerable<decimal> enumerable, int sentimentPeriod)
        {
            return enumerable.Skip(Math.Max(0, enumerable.Count() - sentimentPeriod)).Average();
        }

        private static decimal GetStrengthSumValue(List<OhlcvObject> items)
        {
            var percentageRange = 0.05M;
            var min = items.Select(x => x.Low).Min();
            var max = items.Select(x => x.Low).Max();
            var range = max - min;

            if (items.Last().Close < (min + range * percentageRange))
            {
                return -1 * items[0].Multiplier / 10;
            }

            if (items.Last().Close > (max - range * percentageRange))
            {
                return 1 * items[0].Multiplier / 10;
            }

            return 0;
        }

        private static decimal GetMomentumSumValue(List<OhlcvObject> items)
        {
            var period = 125;
            var source = items.Skip(Math.Max(0, items.Count() - period)).Select(x => x.Close);
            var aver125min = MovingAvg(source, period);

            return (items.Last().Close - aver125min.Last()) * items[0].Multiplier / 100;
        }

        public static IEnumerable<decimal> MovingAvg(IEnumerable<decimal> source, int period)
        {
            var buffer = new Queue<decimal>();

            foreach (var value in source)
            {
                buffer.Enqueue(value);

                // sume the buffer for the average at any given time
                yield return buffer.Sum() / buffer.Count;

                // Dequeue when needed 
                if (buffer.Count == period)
                    buffer.Dequeue();
            }
        }

        private static decimal GetPatternsSumValue(List<OhlcvObject> items)
        {
            Patterns patterns = new Patterns();
            var bullishSignalsCount = patterns.GetBullishSignalsCount(items);
            var bearishSignalsCount = patterns.GetBearishSignalsCount(items);

            var volumeStrength = (decimal)items.Last().Volume / items.Sum(x => x.Volume) / items.Count;

            return ((bullishSignalsCount - bearishSignalsCount) * items[0].Multiplier) * 10000 * volumeStrength;
        }

        private static decimal GetHammersValue(List<OhlcvObject> items)
        {
            var upper = items.Where(x => x.Close == x.High)
                .Select(x => (x.Close - x.Open) / x.Close)
                .Where(x => x > 0.001M);
            var lower = items.Where(x => x.Close == x.Low)
                .Select(x => (x.Close - x.Open) / x.Close)
                .Where(x => x > 0.001M);

            var volumeStrength = (decimal)items.Last().Volume / items.Sum(x => x.Volume) / items.Count;

            return (upper.Sum(x => x) - lower.Sum(x => x)) * items[0].Multiplier * volumeStrength * 100000;
        }

        private static decimal GetGapsValue(List<OhlcvObject> items)
        {
            var gaps = Enumerable.Range(1, items.Count - 1)
                  .Select(i => (items[i].Open - items[i - 1].Close) / items[i - 1].Close)
                  .Where(x => x > 0.005M)
                  .ToList();

            var volumeStrength = (decimal)items.Last().Volume / items.Sum(x => x.Volume) / items.Count;

            return gaps.Sum(x => x) * items[0].Multiplier * volumeStrength * 100000;
        }

        private static decimal GetPeakSumValue(List<OhlcvObject> items)
        {
            var capitalPeakLevel = items.Average(x => x.Capital) * 4;
            var peaks = items.Where(x => x.Capital > capitalPeakLevel && x.Index > 15).Select(x => x).ToList();

            var peaksSum = GetSumOfWeightenedPeaks(peaks, items);

            return peaksSum / 10;
        }

        private static decimal GetSumOfWeightenedPeaks(List<OhlcvObject> peaks, List<OhlcvObject> items)
        {
            List<decimal> peakWeightenedValues = new List<decimal>();
            foreach (var peak in peaks)
            {
                try
                {
                    var averageCloseAfterPeak = items.Where(x => x.Index >= peak.Index && x.Index <= peak.Index + 500).Average(x => x.Close);
                    var peakValue = (peak.Close - averageCloseAfterPeak) / peak.Close * peak.Multiplier * 1; // '* 1' means that if drops down after peak, it is negative

                    peakWeightenedValues.Add(peakValue);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            return peakWeightenedValues.Sum(x => x);
        }

        private static CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
