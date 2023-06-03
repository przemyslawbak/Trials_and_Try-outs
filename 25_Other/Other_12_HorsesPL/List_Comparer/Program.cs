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
            var yearStep = 0.8M;
            int yearNow = DateTime.Now.Year;
            char[] separator = new char[] { '\t' };
            string[] horseRows = File.ReadAllLines("results.txt");
            string[] raceRows = File.ReadAllLines("race.txt");
            string[] jockeyRows = File.ReadAllLines("jockey.txt");
            string[] trenerRows = File.ReadAllLines("trener.txt");
            string[] horsieInfoRows = File.ReadAllLines("horsie.txt");
            string[] fatherRoes = File.ReadAllLines("father.txt");
            string[] motherRoes = File.ReadAllLines("mother.txt");
            string[] sibling1Roes = File.ReadAllLines("sibling1.txt");
            string[] sibling2Roes = File.ReadAllLines("sibling2.txt");
            string[] sibling3Roes = File.ReadAllLines("sibling3.txt");

            var classValues = GetClassValues();
            int raceDistance = int.Parse(raceRows[0].Trim());

            Dictionary<string, decimal> sexWeightDict = new Dictionary<string, decimal>()
            {
                { "klacz", 0.95M },
                { "ogier", 1.0M },
                { "wałach", 0.975M },
            };

            int horsieAge = int.Parse(horsieInfoRows[2].Split('(')[1].Split(' ')[0].ToLower().Trim());
            string horsieSex = horsieInfoRows[2].Split(' ')[1].ToLower();
            string horsieName = horsieInfoRows[1].Trim();

            decimal sexWeight = sexWeightDict[horsieSex];
            decimal ageWeight = (decimal)horsieAge * 0.95M;

            List<decimal> resultsHorse = new List<decimal>();
            List<decimal> resultsJockey = new List<decimal>();
            List<decimal> resultsTrener = new List<decimal>();
            List<decimal> resultsFather = new List<decimal>();
            List<decimal> resultsMother = new List<decimal>();
            List<decimal> resultsSibling1 = new List<decimal>();
            List<decimal> resultsSibling2 = new List<decimal>();
            List<decimal> resultsSibling3 = new List<decimal>();

            foreach (var row in horseRows)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * yearRaceScore * distanceScore;
                    if (raceScore > 0) resultsHorse.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in fatherRoes)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                    if (raceScore > 0) resultsFather.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in motherRoes)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                    if (raceScore > 0) resultsMother.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in sibling1Roes)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                    if (raceScore > 0)
                        resultsSibling1.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in sibling2Roes)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                    if (raceScore > 0)
                        resultsSibling2.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in sibling3Roes)
            {
                try
                {
                    int distance = int.Parse(row.Split(separator)[2].Split(' ')[0].Trim());
                    decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;
                    int yearRace = DateTime.Parse(row.Split(separator)[0]).Year;
                    decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);
                    int catValue = classValues[row.Split(separator)[3]];
                    int place = int.Parse(row.Split(separator)[7].Split('/')[0].Trim());
                    int qty = int.Parse(row.Split(separator)[7].Split('/')[1].Trim());
                    decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                    if (raceScore > 0)
                        resultsSibling3.Add(raceScore);
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in jockeyRows)
            {
                try
                {
                    string horseName = row.Split(separator)[0].Split(' ')[0];
                    decimal nameMultiplier = horseName == horsieName ? 0.7M : 1.0M;

                    if (decimal.Parse(row.Split(separator)[10]) > 0 && (int.Parse(row.Split(separator)[4]) > 0 || int.Parse(row.Split(separator)[5]) > 0 || int.Parse(row.Split(separator)[6]) > 0))
                    {

                        decimal times1 = 0M;
                        decimal times2 = 0M;
                        decimal times3 = 0M;
                        if (int.Parse(row.Split(separator)[4]) > 0) times1 = 1 / decimal.Parse(row.Split(separator)[4]) * decimal.Parse(row.Split(separator)[10]) * 0.1M;
                        if (int.Parse(row.Split(separator)[5]) > 0) times2 = 1 / decimal.Parse(row.Split(separator)[5]) * decimal.Parse(row.Split(separator)[10]) * 0.2M;
                        if (int.Parse(row.Split(separator)[6]) > 0) times3 = 1 / decimal.Parse(row.Split(separator)[6]) * decimal.Parse(row.Split(separator)[10]) * 0.3M;
                        var results = (times1 + times2 + times3) * nameMultiplier;

                        decimal raceScore = nameMultiplier * results;
                        resultsJockey.Add(raceScore);
                    }
                    else if (decimal.Parse(row.Split(separator)[10]) > 0)
                    {
                        resultsJockey.Add(1M * decimal.Parse(row.Split(separator)[10]));
                    }
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            foreach (var row in trenerRows)
            {
                try
                {
                    string horseName = row.Split(separator)[0].Split(' ')[0];
                    decimal nameMultiplier = horseName == horsieName ? 0.7M : 1.0M;

                    if (decimal.Parse(row.Split(separator)[10]) > 0 && (int.Parse(row.Split(separator)[4]) > 0 || int.Parse(row.Split(separator)[5]) > 0 || int.Parse(row.Split(separator)[6]) > 0))
                    {

                        decimal times1 = 0M;
                        decimal times2 = 0M;
                        decimal times3 = 0M;
                        if (int.Parse(row.Split(separator)[4]) > 0) times1 = 1 / decimal.Parse(row.Split(separator)[4]) * decimal.Parse(row.Split(separator)[10]) * 0.1M;
                        if (int.Parse(row.Split(separator)[5]) > 0) times2 = 1 / decimal.Parse(row.Split(separator)[5]) * decimal.Parse(row.Split(separator)[10]) * 0.2M;
                        if (int.Parse(row.Split(separator)[6]) > 0) times3 = 1 / decimal.Parse(row.Split(separator)[6]) * decimal.Parse(row.Split(separator)[10]) * 0.3M;
                        var results = (times1 + times2 + times3) * nameMultiplier;

                        decimal raceScore = nameMultiplier * results;
                        resultsTrener.Add(raceScore);
                    }
                    else if (decimal.Parse(row.Split(separator)[10]) > 0)
                    {
                        resultsTrener.Add(1M * decimal.Parse(row.Split(separator)[10]));
                    }
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            var sibling1core = resultsSibling1.Count > 0 ? (decimal)resultsSibling1.Average(x => x) : 1;
            var sibling2core = resultsSibling2.Count > 0 ? (decimal)resultsSibling2.Average(x => x) : 1;
            var sibling3core = resultsSibling3.Count > 0 ? (decimal)resultsSibling3.Average(x => x) : 1;
            var motherScore = resultsMother.Count > 0 ? (decimal)resultsMother.Average(x => x) : 1;
            var fatherScore = resultsFather.Count > 0 ? (decimal)resultsFather.Average(x => x) : 1;
            var horseScore = resultsHorse.Count > 0 ? (decimal)resultsHorse.Average(x => x) * sexWeight * ageWeight : 1;
            var jockeyScore = resultsJockey.Count > 0 ? (decimal)resultsJockey.Average(x => x) : 1;
            var trenerScore = resultsTrener.Count > 0 ? (decimal)resultsTrener.Average(x => x) : 1;

            System.IO.File.WriteAllText(@"_result.txt", horseScore + "|" + jockeyScore + "|" + trenerScore + "|" + fatherScore + "|" + motherScore + "|" + sibling1core + "|" + sibling2core + "|" + sibling3core + "|" + resultsHorse.Count);
        }

        private static Dictionary<string, int> GetClassValues()
        {
            return new Dictionary<string, int>()
            {
                { "G1 A", 8 },
                { "G1 B", 8 },
                { "L B", 8 },
                { "L A", 8 },
                { "A", 10 },
                { "B", 15 },
                { "I", 20 },
                { "II", 25 },
                { "III", 30 },
                { "", 30 },
                { "IV", 35 },
            };
        }
    }
}
