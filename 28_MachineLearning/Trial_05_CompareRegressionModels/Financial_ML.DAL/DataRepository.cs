using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Financial_ML.DAL
{
    public class DataRepository : IDataRepository
    {
        private readonly string _fileNameDax = "^dax_d.csv";
        private readonly string _fileNameBrent = "brent.csv";

        public List<Quote> GetBrentQuotes()
        {
            string fileName = _fileNameBrent;
            string dataPath = GetPath(fileName);
            return ReadQuotes(dataPath);
        }

        private string GetPath(string fileName)
        {
            Assembly bundleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                             .First(x => x.FullName.Contains("Financial_ML.DAL"));
            string asmPath = new Uri(bundleAssembly.Location).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath), "Data", fileName);
        }

        public List<Quote> GetDaxQuotes()
        {
            string fileName = _fileNameDax;
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            return ReadQuotes(dataPath);
        }

        private List<Quote> ReadQuotes(string dataPath)
        {
            List<Quote> quotes = new List<Quote>();

            using (StreamReader reader = new StreamReader(dataPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    if (values[0].Length == 9)
                    {
                        values[0] = "0" + values[0];
                    }
                    Quote q = new Quote();
                    q.Date = values[0].Split('-')[0].Length == 4 ?
                        DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture) :
                        DateTime.ParseExact(values[0], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    q.Open = decimal.Parse(values[1], CultureInfo.InvariantCulture);
                    q.High = decimal.Parse(values[2], CultureInfo.InvariantCulture);
                    q.Low = decimal.Parse(values[3], CultureInfo.InvariantCulture);
                    q.Close = decimal.Parse(values[4], CultureInfo.InvariantCulture);
                    q.Volume = values.Length == 6 ? decimal.Parse(values[5], CultureInfo.InvariantCulture) : 0;
                    quotes.Add(q);
                }
            }

            return quotes;
        }
    }
}
