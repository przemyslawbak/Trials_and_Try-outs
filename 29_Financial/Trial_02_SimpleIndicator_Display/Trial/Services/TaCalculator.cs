using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Trial.Services
{
    public class TaCalculator : ITaCalculator
    {
        private static readonly string _dataFilename = "jsw_d.csv";

        public List<decimal> GetPrices()
        {
            List<decimal> prices = new List<decimal>();
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", _dataFilename);

            using (StreamReader reader = new StreamReader(dataPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    prices.Add(decimal.Parse(values[4], CultureInfo.InvariantCulture));
                }
            }

            return prices;
        }

        public List<Quote> GetQuotes()
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
