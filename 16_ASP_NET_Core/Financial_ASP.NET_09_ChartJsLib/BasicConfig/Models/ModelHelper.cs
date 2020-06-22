using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BasicConfig.Models
{
    public class ModelHelper
    {
        #region Json string data
        public static string IntervalStringData()
        {
            var d = ModelHelper.IntervalArrayData();
            var cols = new JsonColumn[]
            {
                new JsonColumn {label = "X", type = "number" },
                new JsonColumn {label = "X*Sin(X*X)", type = "number" },
                new JsonColumn {id="i0", type = "number", role = "interval" },
                new JsonColumn {id="i0", type = "number", role = "interval" },
                new JsonColumn {id="i1", type = "number", role = "interval" },
                new JsonColumn {id="i1", type = "number", role = "interval" },
                new JsonColumn {id="i2", type = "number", role = "interval" },
                new JsonColumn {id="i2", type = "number", role = "interval" }
            };

            var rows = new JsonRow[d.Count - 1];
            for (int i = 1; i < d.Count; i++)
            {
                var obj = d[i] as object[];

                rows[i - 1] = new JsonRow
                {
                    c = new JsonCell[]
                    {
                        new JsonCell { v = obj[0] },
                        new JsonCell { v = obj[1] },
                        new JsonCell { v = obj[2] },
                        new JsonCell { v = obj[3] },
                        new JsonCell { v = obj[4] },
                        new JsonCell { v = obj[5] },
                        new JsonCell { v = obj[6] },
                        new JsonCell { v = obj[7] }
                    }
                };

            }
            return JsonString.GetGoogleJson(cols, rows);
        }





        public static string TaskStringData()
        {
            var data = TaskArrayData();
            var cols = new JsonColumn[]
            {
                new JsonColumn{label = "Task", type = "string"},
                new JsonColumn{label = "Hours per Day", type = "number"}
            };

            var rows = new JsonRow[data.Count - 1];
            for (int i = 1; i < data.Count; i++)
            {
                var obj = data[i] as object[];
                rows[i - 1] = new JsonRow
                {
                    c = new JsonCell[] { new JsonCell { v = obj[0] }, new JsonCell { v = obj[1] } }
                };
            }
            return JsonString.GetGoogleJson(cols, rows);
        }

        public static string MultilineStringData()
        {
            var data = MultilineArrayData();
            var cols = new JsonColumn[]
            {
                new JsonColumn {label = "X", type = "number" },
                new JsonColumn {label = "Sin(X)", type = "number" },
                new JsonColumn {label = "Cos(X)", type = "number" },
                new JsonColumn {label = "Sin(X)^2", type = "number" }
            };

            var rows = new JsonRow[data.Count - 1];
            for (int i = 1; i < data.Count; i++)
            {
                var obj = data[i] as object[];
                rows[i - 1] = new JsonRow
                {
                    c = new JsonCell[]
                    {
                        new JsonCell{ v = obj[0] },
                        new JsonCell{ v = obj[1] },
                        new JsonCell{ v = obj[2] },
                        new JsonCell{ v = obj[3] }
                    }
                };
            }
            return JsonString.GetGoogleJson(cols, rows);
        }
        #endregion Json string data



        #region Json array data

        public static List<object> PopulationArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "Country", "Continent", "% of World Population", "Population (Million)" });
            objs.Add(new object[] { "China", "Asia", 19.2, 1401 });
            objs.Add(new object[] { "India", "Asia", 17.5, 1282 });
            objs.Add(new object[] { "United States", "N. America", 4.45, 325 });
            objs.Add(new object[] { "Indonesia", "Asia", 3.49, 256 });
            objs.Add(new object[] { "Brazil", "S. America", 2.79, 204 });
            objs.Add(new object[] { "Pakistan", "Asia", 2.56, 188 });
            objs.Add(new object[] { "Nigeria", "Africa", 2.46, 184 });
            objs.Add(new object[] { "Bangladesh", "Asia", 2.18, 160 });
            objs.Add(new object[] { "Russia", "Europe", 1.97, 142 });
            objs.Add(new object[] { "Japan", "Asia", 1.75, 127 });
            objs.Add(new object[] { "Mexico", "N. America", 1.71, 125 });
            objs.Add(new object[] { "Philippines", "Asia", 1.38, 102 });
            objs.Add(new object[] { "Ethiopia", "Africa", 1.33, 99 });
            objs.Add(new object[] { "Vietnam", "Asia", 1.28, 93 });
            objs.Add(new object[] { "Egypt", "Africa", 1.15, 85 });
            objs.Add(new object[] { "Germany", "Europe", 1.14, 83 });
            objs.Add(new object[] { "Iran", "Asia", 1.08, 79 });
            objs.Add(new object[] { "Turkey", "Asia", 1.05, 77 });
            objs.Add(new object[] { "Congo", "Africa", 0.1, 71 });
            return objs;
        }


        public static List<object> RandomArrayData(Random random)
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "X", "Y", "Z" });
            for (int i = 0; i < 20; i++)
            {
                var x = random.Next(0, 20);
                var y = random.Next(0, 100);
                var z = random.Next(0, 100);
                objs.Add(new object[] { x, y, z });
            }
            return objs;
        }

        public static List<object> IntervalArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "x", "y", "y1", "y2", "y3", "y4", "y5", "y6" });
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                double ran = random.NextDouble();
                double x = 1.0 * i;
                double y = x * Math.Sin(0.1 * x * x) + 10.0;
                double y1 = y + 1.0 * ran;
                double y2 = y - 1.0 * ran;
                double y3 = y + 2.0 * ran;
                double y4 = y - 2.0 * ran;
                double y5 = y + 3.0 * ran;
                double y6 = y - 3.0 * ran;

                objs.Add(new object[] { x, y, y1, y2, y3, y4, y5, y6 });
            }
            return objs;
        }


        public static List<object> AreaArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "x", "1.2+Sin(x)", "1.2+Cos(x)", "1.2+Sin(x)^2" });
            for (int i = 0; i < 70; i++)
            {
                double x = 0.1 * i;
                objs.Add(new object[] { x, 1.2 + Math.Sin(x), 1.2 + Math.Cos(x), 1.2 + Math.Sin(x) * Math.Sin(x) });
            }
            return objs;
        }


        public static List<object> GroupBarArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "Year", "Houston", "Los Angeles", "New York", "Chicago", "Philadephia" });
            objs.Add(new object[] { "1990", 1.70, 3.49, 7.32, 2.78, 1.59 });
            objs.Add(new object[] { "2000", 1.98, 3.70, 8.02, 2.90, 1.51 });
            objs.Add(new object[] { "2005", 2.08, 3.79, 8.21, 2.82, 1.52 });
            objs.Add(new object[] { "2010", 2.10, 3.79, 8.18, 2.70, 1.53 });
            objs.Add(new object[] { "2015", 2.30, 3.97, 8.55, 2.72, 1.57 });
            return objs;
        }

        public static List<object> BarArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "City", "2010 Population" });
            objs.Add(new object[] { "New York City", 8175000 });
            objs.Add(new object[] { "Los Angles", 3792000 });
            objs.Add(new object[] { "Chicago", 2695000 });
            objs.Add(new object[] { "Houston", 2099000 });
            objs.Add(new object[] { "Philadephia", 1526000 });
            return objs;
        }

        public static List<object> TaskArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "Task", "Hours" });
            objs.Add(new object[] { "Work", 11 });
            objs.Add(new object[] { "Eat", 2 });
            objs.Add(new object[] { "Commute", 5 });
            objs.Add(new object[] { "Watch TV", 4 });
            objs.Add(new object[] { "Sleep", 7 });
            return objs;
        }

        public static List<object> MultilineArrayData()
        {
            List<object> objs = new List<object>();
            objs.Add(new object[] { "x", "Sin(x)", "Cos(x)", "Sin(x)^2" });
            for (int i = 0; i < 70; i++)
            {
                double x = 0.1 * i;
                objs.Add(new object[] { x, Math.Sin(x), Math.Cos(x), Math.Sin(x) * Math.Sin(x) });
            }
            return objs;
        }

        public static object ChartJsData(int numPoints)
        {
            List<object> x = new List<object>();
            List<object> y1 = new List<object>();
            List<object> y2 = new List<object>();
            List<object> y3 = new List<object>();

            double dx = 2.0 * Math.PI / numPoints;

            for (double xx = 0; xx <= 2.0 * Math.PI; xx += dx)
            {
                x.Add(Math.Round(xx, 2));
                y1.Add(Math.Sin(xx));
                y2.Add(Math.Cos(xx));
                y3.Add(2 * Math.Sin(xx) * Math.Sin(xx));
            }

            return new { x, y1, y2, y3 };
        }
        #endregion Json array data


        public static List<object> CsvToIndexData()
        {
            string path = AppContext.BaseDirectory;
            string[] ss = Regex.Split(path, "bin");
            string filePath = ss[0] + @"Models\";
            string csvFile = filePath + "indices.csv";
            FileStream fs = new FileStream(csvFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            List<String> lst = new List<string>();
            while (!sr.EndOfStream)
                lst.Add(sr.ReadLine());

            string[] fields = lst[0].Split(new char[] { ',' });
            var res = new List<object>();

            for (int i = 1; i < lst.Count; i++)
            {
                fields = lst[i].Split(',');
                res.Add(new
                {
                    Date = DateTime.Parse(fields[0]),
                    IGSpread = double.Parse(fields[1]),
                    HYSpread = double.Parse(fields[2]),
                    SPX = double.Parse(fields[3]),
                    VIX = double.Parse(fields[4])
                });
            }
            return res;
        }

        public static List<object> CsvToPopulationData()
        {
            string path = AppContext.BaseDirectory;
            string[] ss = Regex.Split(path, "bin");
            string filePath = ss[0] + @"Models\";
            string csvFile = filePath + "population.csv";
            FileStream fs = new FileStream(csvFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            List<String> lst = new List<string>();
            while (!sr.EndOfStream)
                lst.Add(sr.ReadLine());

            string[] fields = lst[0].Split(new char[] { ',' });
            var res = new List<object>();

            for (int i = 1; i < lst.Count; i++)
            {
                fields = lst[i].Split(',');
                res.Add(new
                {
                    Rank = fields[0],
                    Country_Territory = fields[1],
                    Year2014 = fields[2],
                    Year2015 = fields[3],
                    Change = fields[4],
                    Continent = fields[5]
                });
            }
            return res;
        }

    }
}
