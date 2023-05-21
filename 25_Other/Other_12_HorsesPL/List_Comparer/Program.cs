using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<decimal> results = new List<decimal>();
            char[] separator = new char[] { '\t' };
            string[] fileRows = File.ReadAllLines("1.txt");
            var classValues = GetClassValues();

            foreach (var row in fileRows)
            {
                try
                {
                    int catValue = classValues[row.Split(separator)[0]];
                    int place = int.Parse(row.Split(separator)[4].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[4].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty;
                    results.Add(raceScore);
                }
                catch
                {
                    //do nothing
                }
            }

            var score = (decimal)results.Sum(x => x) / (decimal)results.Count / 10;
        }

        private static Dictionary<string, int> GetClassValues()
        {
            return new Dictionary<string, int>()
            {
                { "A", 10 },
                { "B", 15 },
                { "I", 20 },
                { "II", 25 },
                { "III", 30 },
                { "IV", 35 },
            };
        }
    }
}
