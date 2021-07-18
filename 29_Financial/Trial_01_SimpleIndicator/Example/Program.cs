using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.IO;

namespace Example
{
    class Program
    {
        private static readonly string _dataFilename = "jsw_d.csv";
        static void Main(string[] args)
        {
            IEnumerable<Quote> quotes = GetHistory();
            IEnumerable<SmaResult> results = quotes.GetSma(10);
        }

        private static IEnumerable<Quote> GetHistory()
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", _dataFilename);
            List<Quote> quotes = new List<Quote>();

            using (StreamReader reader = new StreamReader(dataPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    try
                    {
                        quotes.Add(new Quote()
                        {
                            Date = DateTime.Parse(values[0]),
                            Open = decimal.Parse(values[1]),
                            High = decimal.Parse(values[2]),
                            Low = decimal.Parse(values[3]),
                            Close = decimal.Parse(values[4]),
                            Volume = decimal.Parse(values[5]),
                        });
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }

            return quotes;
        }
    }
}
