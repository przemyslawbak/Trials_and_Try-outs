using System;
using System.Collections.Generic;
using System.Globalization;
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
            string[] sizeRows = File.ReadAllLines("size.txt");

            string[] urls = File.ReadAllLines("link.txt");
            var urlHorse = urls[0];

            var urlJockeyList = new List<string>()
            {
                urls[1] + "&sezon=2023#wyniki_koni",
                urls[1] + "&sezon=2022#wyniki_koni",
                urls[1] + "&sezon=2021#wyniki_koni",
                urls[1] + "&sezon=2020#wyniki_koni",
            };
            var urlTrainerList = new List<string>()
            {
                urls[2] + "&sezon=2023#wyniki_koni",
                urls[2] + "&sezon=2022#wyniki_koni",
                urls[2] + "&sezon=2021#wyniki_koni",
                urls[2] + "&sezon=2020#wyniki_koni",
            };

            var jr = new List<string>();
            var tr = new List<string>();

            foreach (var urlJockey in urlJockeyList)
            {
                string htmlJockey = GetHtml(urlJockey);
                var jockeyStarts = htmlJockey
                .Split(new string[] { "<h3 class=\"g-color-black g-font-weight-600 mb-3\">WYNIKI KONI DOSIADANYCH W SEZONIE " }, StringSplitOptions.None)[1]
                .Split(new string[] { "<tbody>" }, StringSplitOptions.None)[1]
                .Split(new string[] { "WYKAZ STARTÓW W SEZONIE " }, StringSplitOptions.None)[0];
                var rows = jockeyStarts
                .Split(new string[] { "<tr>" }, StringSplitOptions.None);

                jr.AddRange(rows);
            }

            foreach (var urlTrainer in urlTrainerList)
            {
                string htmlTrainer = GetHtml(urlTrainer);

                var trainerStarts = htmlTrainer
                .Split(new string[] { "WYNIKI KONI TRENOWANYCH W SEZONIE " }, StringSplitOptions.None)[1]
                .Split(new string[] { "<tbody>" }, StringSplitOptions.None)[1];

                var rows = trainerStarts
                    .Split(new string[] { "<tr>" }, StringSplitOptions.None);

                tr.AddRange(rows);
            }

            var jockeyRows = jr.ToArray();
            var trainerRows = tr.ToArray();

            var classValues = GetClassValues();
            int raceDistance = int.Parse(raceRows[0].Trim());

            var sizeHeight = sizeRows[0].Split('-')[0]; //(200 - 161) : 2 = 19,5; (200-157) : 2 = 21,5
            var sizeChest = sizeRows[0].Split('-')[1]; //200-181 = 19; 200-178 = 21
            var sizeLeg = sizeRows[0].Split('-')[2]; //1 : 22 * 400 = 18,2; 1 : 19 * 300 = 21

            var sizeRatio = ((200 - decimal.Parse(sizeHeight, CultureInfo.InvariantCulture)) / 2) + (200 - decimal.Parse(sizeChest, CultureInfo.InvariantCulture)) + (1 / decimal.Parse(sizeLeg, CultureInfo.InvariantCulture) * 400);

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
            decimal ageWeight = 1 + (decimal)horsieAge * 0.05M;
            if (obstructions) classValues["płoty"] = 5;
            if (obstructions) classValues["przeszkody"] = 5;

            List<decimal> resultsHorse = new List<decimal>();
            List<decimal> resultsFather = new List<decimal>();
            List<decimal> resultsMother = new List<decimal>();
            List<decimal> siblingResults = new List<decimal>();
            List<decimal> resultsJockey = new List<decimal>();
            List<decimal> resultsTrainer = new List<decimal>();

            Console.WriteLine("Computing horse score...");

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

            string htmlHorse = GetHtml(urlHorse);

            if (jockeyRows.Length > 0) jockeyRows = jockeyRows.Take(jockeyRows.Count() - 1).ToArray();

            Console.WriteLine("Computing jockeys score...");

            foreach (var row in jockeyRows)
            {
                try
                {
                    var horseName = row
                        .Split('>')[2]
                        .Split('(')[0].Trim();

                    decimal nameMultiplier = horseName.ToLower() == horsieName.ToLower() ? 0.7M : 1.0M;

                    var placesFirst = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[4].Split('<')[0];
                    var placesSecond = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[5].Split('<')[0];
                    var placesThird = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[6].Split('<')[0];
                    var races = row.Split(new string[] { "<td style=\"text-align: center\">" }, StringSplitOptions.None)[1].Split('<')[0];

                    if (races != "0" && (placesFirst != "0" || placesSecond != "0" || placesThird != "0"))
                    {
                        decimal times1 = 0M;
                        decimal times2 = 0M;
                        decimal times3 = 0M;
                        if (int.Parse(placesFirst) > 0) times1 = 1 / decimal.Parse(placesFirst) * decimal.Parse(races) * 0.1M;
                        if (int.Parse(placesSecond) > 0) times2 = 1 / decimal.Parse(placesSecond) * decimal.Parse(races) * 0.2M;
                        if (int.Parse(placesThird) > 0) times3 = 1 / decimal.Parse(placesThird) * decimal.Parse(races) * 0.3M;
                        var results = (times1 + times2 + times3) * nameMultiplier;

                        decimal raceScore = nameMultiplier * results;
                        resultsJockey.Add(raceScore);
                    }
                    else if (decimal.Parse(races) > 0)
                    {
                        resultsJockey.Add(1M * decimal.Parse(races));
                    }
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            if (trainerRows.Length > 0) trainerRows = trainerRows.Take(trainerRows.Count() - 1).ToArray();

            Console.WriteLine("Computing trainers score...");

            foreach (var row in trainerRows)
            {
                try
                {
                    var horseName = row
                        .Split('>')[2]
                        .Split('(')[0].Trim();

                    decimal nameMultiplier = horseName.ToLower() == horsieName.ToLower() ? 0.7M : 1.0M;

                    var placesFirst = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[4].Split('<')[0];
                    var placesSecond = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[5].Split('<')[0];
                    var placesThird = row.Split(new string[] { "<td style=\"text-align:center\">" }, StringSplitOptions.None)[6].Split('<')[0];
                    var races = row.Split(new string[] { "<td style=\"text-align: center\">" }, StringSplitOptions.None)[1].Split('<')[0];

                    if (races != "0" && (placesFirst != "0" || placesSecond != "0" || placesThird != "0"))
                    {
                        decimal times1 = 0M;
                        decimal times2 = 0M;
                        decimal times3 = 0M;
                        if (int.Parse(placesFirst) > 0) times1 = 1 / decimal.Parse(placesFirst) * decimal.Parse(races) * 0.1M;
                        if (int.Parse(placesSecond) > 0) times2 = 1 / decimal.Parse(placesSecond) * decimal.Parse(races) * 0.2M;
                        if (int.Parse(placesThird) > 0) times3 = 1 / decimal.Parse(placesThird) * decimal.Parse(races) * 0.3M;
                        var results = (times1 + times2 + times3) * nameMultiplier;

                        decimal raceScore = nameMultiplier * results;
                        resultsTrainer.Add(raceScore);
                    }
                    else if (decimal.Parse(races) > 0)
                    {
                        resultsTrainer.Add(1M * decimal.Parse(races));
                    }
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }


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

            Console.WriteLine("Computing fathers score...");

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

            Console.WriteLine("Computing mothers score...");

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


            var startyMotherHtml = htmlMother.Split(new string[] { "<th style=\"text-align:right\" scope=\"col\">wygrana</th>" }, StringSplitOptions.None)[1];
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

            Console.WriteLine("Computing siblings score...");

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

            var siblingsScore = siblingResults.Count > 0 ? (decimal)siblingResults.Average(x => x) : 1;
            var motherScore = resultsMother.Count > 0 ? (decimal)resultsMother.Average(x => x) : 1;
            var fatherScore = resultsFather.Count > 0 ? (decimal)resultsFather.Average(x => x) : 1;
            var horseScore = resultsHorse.Count > 0 ? (decimal)resultsHorse.Average(x => x) * sexWeight * ageWeight : 1;
            var jockeyScore = resultsJockey.Count > 0 ? (decimal)resultsJockey.Average(x => x) : 1;
            var trenerScore = resultsTrainer.Count > 0 ? (decimal)resultsTrainer.Average(x => x) : 1;

            File.WriteAllText(@"_result.txt", horsieName + "|" + horseScore + "|" + fatherScore + "|" + motherScore + "|" + siblingsScore + "|" + jockeyScore + "|" + trenerScore + "|" + sizeRatio + "|" + resultsHorse.Count);

            Console.WriteLine("Finito...");
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
