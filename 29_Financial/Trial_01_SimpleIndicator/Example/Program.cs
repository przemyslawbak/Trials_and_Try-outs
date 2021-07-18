using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        private static readonly string _dataFilename = "jsw_d.csv";
        static void Main(string[] args)
        {
            List<Quote> quotes = GetHistory();
            List<SmaResult> results = quotes.GetSma(10).ToList();
            SmaResult result = results.LastOrDefault();
            for (int i = 0; i < quotes.Count(); i++)
            {
                Console.WriteLine("SMA on {0} was PLN{1}", quotes[i].Close, results[i].Sma);
            }
            
        }

        private static List<Quote> GetHistory()
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", _dataFilename);
            List<Quote> quotes = new List<Quote>();

            using (StreamReader reader = new StreamReader(dataPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    Quote q = new Quote();
                    q.Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    q.Open = decimal.Parse(values[1], CultureInfo.InvariantCulture);
                    q.High = decimal.Parse(values[2], CultureInfo.InvariantCulture);
                    q.Low = decimal.Parse(values[3], CultureInfo.InvariantCulture);
                    q.Close = decimal.Parse(values[4], CultureInfo.InvariantCulture);
                    q.Volume = decimal.Parse(values[5], CultureInfo.InvariantCulture);
                    quotes.Add(q);
                }
            }

            return quotes;
        }
    }
}
