using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var obstructions = true;
            var yearStep = 0.8M;
            int yearNow = DateTime.Now.Year;
            char[] separator = new char[] { '\t' };
            string[] raceRows = File.ReadAllLines("race.txt");
            string[] horsieInfoRows = File.ReadAllLines("horsie.txt");
            string[] horseRows = File.ReadAllLines("results.txt");


            string[] urlHorse = File.ReadAllLines("link.txt");

            var classValues = GetClassValues();
            int raceDistance = int.Parse(raceRows[0].Trim());

            Dictionary<string, decimal> sexWeightDict = new Dictionary<string, decimal>()
            {
                { "klacz", 0.95M },
                { "ogier", 1.0M },
                { "wałach", 0.975M },
            };

            int horsieAge = int.Parse(horsieInfoRows[1].Split('(')[1].Split(' ')[0].ToLower().Trim());
            string horsieSex = horsieInfoRows[1].Split(' ')[1].ToLower();
            string horsieName = horsieInfoRows[0].Trim();

            decimal sexWeight = sexWeightDict[horsieSex];
            decimal ageWeight = 1 + (decimal)horsieAge * 0.05M;
            if (obstructions) classValues["płoty"] = 5;
            if (obstructions) classValues["przeszkody"] = 5;

            List<decimal> resultsHorse = new List<decimal>();
            List<decimal> resultsFather = new List<decimal>();
            List<decimal> resultsMother = new List<decimal>();
            List<decimal> siblingResults = new List<decimal>();

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

            string htmlHorse = GetHtml(urlHorse[0]);

            var urlFather = htmlHorse.Split(new string[] { "<td><a href=\"" }, StringSplitOptions.None)[1].Split('"')[0];
            if (urlFather.Length > 10)
            {
                urlFather = "https://koniewyscigowe.pl" + urlFather;
            }
            else
            {
                urlFather = "https://koniewyscigowe.pl" + "horse/" + urlFather;
            }

            var urlMother = htmlHorse.Split(new string[] { "<td><a href=\"" }, StringSplitOptions.None)[2].Split('"')[0];
            if (urlMother.Length > 10)
            {
                urlMother = "https://koniewyscigowe.pl" + urlMother;
            }
            else
            {
                urlMother = "https://koniewyscigowe.pl" + "/horse/" + urlMother;
            }

            string htmlFather = GetHtml(urlFather);
            string htmlMother = GetHtml(urlMother);

            var potomstwoUrlFather = new List<string>();
            var potomstwoHtmlFather = htmlFather.Split(new string[] { "<table class=\"table table-striped\" id=\"wykaz\">" }, StringSplitOptions.None)[2].Split(new string[] { "</tbody>" }, StringSplitOptions.None)[0];
            var potomstwoPreFather = potomstwoHtmlFather.Split(new string[] { "<td style=\"text-align:center\"><a href=\"" }, StringSplitOptions.None).Select(x => x.Split('"')[0]).ToList();
            potomstwoPreFather.RemoveAt(0);

            List<string> siblingsFatherUrls = new List<string>();
            for (int i = 0; i < potomstwoPreFather.Count;  i++)
            {
                if (i % 3 == 0)
                {
                    if (potomstwoPreFather[i].Length > 10)
                    {
                        potomstwoPreFather[i] = "https://koniewyscigowe.pl" + potomstwoPreFather[i];
                    }
                    else
                    {
                        potomstwoPreFather[i] = "https://koniewyscigowe.pl" + "/horse/" + potomstwoPreFather[i];
                    }
                    potomstwoUrlFather.Add(potomstwoPreFather[i]);
                }
            }

            var potomstwoUrlMother = new List<string>();
            var potomstwoHtmlMother = htmlMother.Split(new string[] { "<table class=\"table table-striped\" id=\"wykaz\">" }, StringSplitOptions.None)[2].Split(new string[] { "</tbody>" }, StringSplitOptions.None)[0];
            var potomstwoPreMother = potomstwoHtmlMother.Split(new string[] { "<td style=\"text-align:center\"><a href=\"" }, StringSplitOptions.None).Select(x => x.Split('"')[0]).ToList();
            potomstwoPreMother.RemoveAt(0);

            List<string> siblingsMotherUrls = new List<string>();
            for (int i = 0; i < potomstwoPreMother.Count;  i++)
            {
                if (i % 3 == 0)
                {
                    if (potomstwoPreMother[i].Length > 10)
                    {
                        potomstwoPreMother[i] = "https://koniewyscigowe.pl" + potomstwoPreMother[i];
                    }
                    else
                    {
                        potomstwoPreMother[i] = "https://koniewyscigowe.pl" + "/horse/" + potomstwoPreMother[i];
                    }
                    potomstwoUrlMother.Add(potomstwoPreMother[i]);
                }
            }

            var startyHorseHtml = htmlHorse.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
            var startyHorseArray = startyHorseHtml.Split(new string[] { "<td style=\"text-align:center\"> <span>" }, StringSplitOptions.None).ToList();
            startyHorseArray.RemoveAt(0);


            var startyFatherHtml = htmlFather.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
            var startyFatherArray = startyFatherHtml.Split(new string[] { "<td style=\"text-align:center\"> <span>" }, StringSplitOptions.None).ToList();
            startyFatherArray.RemoveAt(0);


            var startyMotherHtml = htmlFather.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
            var startyMotherArray = startyMotherHtml.Split(new string[] { "<td style=\"text-align:center\"> <span>" }, StringSplitOptions.None).ToList();
            startyMotherArray.RemoveAt(0);

            //ok
            foreach (var row in startyHorseArray)
            {
                if (startyHorseArray.Count > 0)
                {
                    try
                    {
                        string distStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[2].Split(new string[] { "&nbsp;m" }, StringSplitOptions.None)[0].Trim();
                        int distance = int.Parse(distStr);
                        decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;

                        string yearStr = row.Split('.')[2].Split('<')[0].Trim();
                        int yearRace = int.Parse(yearStr);
                        decimal yearRaceScore = (1 + (yearNow - yearRace) * yearStep);

                        string catStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[3].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int catValue = classValues[catStr];

                        string placeStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[7].Split(new string[] { " / " }, StringSplitOptions.None)[0].Trim();
                        string qtyStr = row.Split(new string[] { " / " }, StringSplitOptions.None)[1].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int place = int.Parse(placeStr);
                        int qty = int.Parse(qtyStr);

                        if (place == 0)
                        {
                            place = qty;
                        }
                        decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * yearRaceScore * distanceScore;
                        if (raceScore > 0) resultsHorse.Add(raceScore);
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }
                }
            }

            foreach (var row in startyFatherArray)
            {
                if (startyFatherArray.Count > 0)
                {
                    try
                    {
                        string distStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[2].Split(new string[] { "&nbsp;m" }, StringSplitOptions.None)[0].Trim();
                        int distance = int.Parse(distStr);
                        decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;

                        string catStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[3].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int catValue = classValues[catStr];

                        string placeStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[7].Split(new string[] { " / " }, StringSplitOptions.None)[0].Trim();
                        string qtyStr = row.Split(new string[] { " / " }, StringSplitOptions.None)[1].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int place = int.Parse(placeStr);
                        int qty = int.Parse(qtyStr);

                        if (place == 0)
                        {
                            place = qty;
                        }
                        decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                        if (raceScore > 0) resultsFather.Add(raceScore);
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }
                }
            }

            foreach (var row in startyMotherArray)
            {
                if (startyMotherArray.Count > 0)
                {
                    try
                    {
                        string distStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[2].Split(new string[] { "&nbsp;m" }, StringSplitOptions.None)[0].Trim();
                        int distance = int.Parse(distStr);
                        decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;

                        string catStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[3].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int catValue = classValues[catStr];

                        string placeStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[7].Split(new string[] { " / " }, StringSplitOptions.None)[0].Trim();
                        string qtyStr = row.Split(new string[] { " / " }, StringSplitOptions.None)[1].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                        int place = int.Parse(placeStr);
                        int qty = int.Parse(qtyStr);

                        if (place == 0)
                        {
                            place = qty;
                        }
                        decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                        if (raceScore > 0) resultsMother.Add(raceScore);
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }
                }
            }

            foreach (var url in potomstwoUrlFather)
            {
                var htmlSibling = GetHtml(url);
                var startySiblingHtml = htmlSibling.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
                var startySiblingArray = startySiblingHtml.Split(new string[] { "<td style=\"text-align:center\"> <span>" }, StringSplitOptions.None).ToList();
                startySiblingArray.RemoveAt(0);
                List<decimal> resultsSibling = new List<decimal>();

                foreach (var row in startySiblingArray)
                {
                    if (startySiblingArray.Count > 0)
                    {
                        try
                        {
                            string distStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[2].Split(new string[] { "&nbsp;m" }, StringSplitOptions.None)[0].Trim();
                            int distance = int.Parse(distStr);
                            decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;

                            string catStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[3].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                            int catValue = classValues[catStr];

                            string placeStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[7].Split(new string[] { " / " }, StringSplitOptions.None)[0].Trim();
                            string qtyStr = row.Split(new string[] { " / " }, StringSplitOptions.None)[1].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                            int place = int.Parse(placeStr);
                            int qty = int.Parse(qtyStr);

                            if (place == 0)
                            {
                                place = qty;
                            }
                            decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                            if (raceScore > 0) resultsSibling.Add(raceScore);
                        }
                        catch (Exception ex)
                        {
                            //do nothing
                        }


                        var siblingScore = resultsSibling.Count > 0 ? (decimal)resultsSibling.Average(x => x) : 1;
                        siblingResults.Add(siblingScore);
                    }
                }
            }

            foreach (var url in potomstwoUrlMother)
            {
                var htmlSibling = GetHtml(url);
                var startySiblingHtml = htmlSibling.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
                var startySiblingArray = startySiblingHtml.Split(new string[] { "<td style=\"text-align:center\"> <span>" }, StringSplitOptions.None).ToList();
                startySiblingArray.RemoveAt(0);
                List<decimal> resultsSibling = new List<decimal>();

                foreach (var row in startySiblingArray)
                {
                    if (startySiblingArray.Count > 0)
                    {
                        try
                        {
                            string distStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[2].Split(new string[] { "&nbsp;m" }, StringSplitOptions.None)[0].Trim();
                            int distance = int.Parse(distStr);
                            decimal distanceScore = 1 + (decimal)Math.Abs(raceDistance - distance) / raceDistance * 10;

                            string catStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[3].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                            int catValue = classValues[catStr];

                            string placeStr = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[7].Split(new string[] { " / " }, StringSplitOptions.None)[0].Trim();
                            string qtyStr = row.Split(new string[] { " / " }, StringSplitOptions.None)[1].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim();
                            int place = int.Parse(placeStr);
                            int qty = int.Parse(qtyStr);

                            if (place == 0)
                            {
                                place = qty;
                            }
                            decimal raceScore = (decimal)catValue * (decimal)place / (decimal)qty * distanceScore;
                            if (raceScore > 0) resultsSibling.Add(raceScore);
                        }
                        catch (Exception ex)
                        {
                            //do nothing
                        }


                        var siblingScore = resultsSibling.Count > 0 ? (decimal)resultsSibling.Average(x => x) : 1;
                        siblingResults.Add(siblingScore);
                    }
                }
            }








/*
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
            }*/
            var siblingsScore = siblingResults.Count > 0 ? (decimal)siblingResults.Average(x => x) : 1;
            var motherScore = resultsMother.Count > 0 ? (decimal)resultsMother.Average(x => x) : 1;
            var fatherScore = resultsFather.Count > 0 ? (decimal)resultsFather.Average(x => x) : 1;
            var horseScore = resultsHorse.Count > 0 ? (decimal)resultsHorse.Average(x => x) * sexWeight * ageWeight : 1;

            System.IO.File.WriteAllText(@"_result.txt", horsieName + "|" + horseScore + "|" + fatherScore + "|" + motherScore + "|" + siblingsScore + "|" + resultsHorse.Count);
        }

        private static Dictionary<string, int> GetClassValues()
        {
            return new Dictionary<string, int>()
            {
                { "LR A", 8 },
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
                { "płoty", 10 },
                { "sulki", 10 },
                { "przeszkody", 10 },
            };
        }

        private static string GetHtml(string url)
        {
            string htmlHorse = "";

            if (!string.IsNullOrEmpty(url))
            {
                using (WebClient client = new WebClient())
                {
                    htmlHorse = client.DownloadString(url);
                }
            }

            return htmlHorse;
        }
    }
}
