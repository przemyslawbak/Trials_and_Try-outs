using System;
using System.Collections.Generic;

namespace BasicConfig.Models
{
    public class ModelHelper
    {
        public static object PolarData()
        {
            List<object> data1 = new List<object>();
            List<object> data2 = new List<object>();
            List<object> data3 = new List<object>();
            List<object> data4 = new List<object>();
            for (int i = 0; i < 720; i++)
            {
                double theta = i * Math.PI / 60;
                data1.Add(new { angle = theta, r = Math.Cos(4.0 * theta) });
                data2.Add(new { angle = theta, r = Math.Cos(theta / 6.0) });
                data3.Add(new { angle = theta, r = Math.Cos(5.0 * theta / 6.0) });
                data4.Add(new { angle = theta, r = Math.Cos(7.0 * theta / 2.0) });
            }
            return new { data1, data2, data3, data4 };
        }

        public static List<object> AreaData()
        {
            List<object> dat = new List<object>();
            for (int i = 0; i < 41; i++)
            {
                double x = 0.25 * i;
                double sn1 = 2.0 + Math.Sin(x);
                double cs = 2.0 + Math.Cos(x);
                double sn2 = 3.0 + Math.Sin(x);
                dat.Add(new { x = x, y = sn1, y1 = cs, y2 = sn2 });
            }
            return dat;
        }


        public static List<object> BarData1()
        {
            List<object> dat = new List<object>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                dat.Add(new { x = i * i, y = Math.Round(random.NextDouble(), 2) });
            }
            return dat;
        }

        public static List<object> BarData2()
        {
            List<object> dat = new List<object>();
            string[] letters = new string[] { "A", "D", "CD", "F", "G", "IH", "JA", "JB", "IX", "YX" };
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                dat.Add(new { x = letters[i], y = random.Next(100) * random.Next(100) });
            }
            return dat;
        }

        public static List<object> BarData3()
        {
            List<object> dat = new List<object>();
            string[] group = new string[] { "y1", "y2", "y3" };
            Random random = new Random();

            double[] xa = new double[5];

            for (int i = 0; i < group.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    xa[j] = Math.Round(random.NextDouble(), 4);
                }
                dat.Add(new { group = group[i], a = xa[0], b = xa[1], c = xa[2], d = xa[3], e = xa[4] });
            }
            return dat;
        }


        public static List<object> Y2Data()
        {
            List<object> dat = new List<object>();
            for (int i = 0; i < 101; i++)
            {
                double x = i / 3.0;
                double y = 0.1 * x * Math.Cos(x);
                double x1 = x / 3.0;
                double y2 = 0.53 * x1 * x1 * x1 - 5.3 * x1 * x1 + 15.77 * x1;
                dat.Add(new { x, y, y2 });
            }
            return dat;
        }

        public static List<object> MultiLineDataForY2()
        {
            List<object> dat = new List<object>();
            for (int i = 0; i < 101; i++)
            {
                double x = i / 3.0;
                double y = 0.1 * x * Math.Cos(x);
                double x1 = x / 3.0;
                double y2 = 0.53 * x1 * x1 * x1 - 5.3 * x1 * x1 + 15.77 * x1;
                dat.Add(new { id = "x*cos(x)", x = x, y = y });
                dat.Add(new { id = "x^3 +...", x = x, y = y2 });
            }
            return dat;
        }

        public static List<object> MultiLineData()
        {
            List<object> dat = new List<object>();
            for (int i = 0; i < 101; i++)
            {
                double x = 0.1 * i;
                double y1 = Math.Sin(x);
                double y2 = 1.0 / ((x - 5) * (x - 5) + 1.0);
                double y3 = 0.1 * x * Math.Sin(0.1 * x * x) + 0.1;
                dat.Add(new { id = "Sine", x = x, y = y1 });
                dat.Add(new { id = "1/(x*x+1)", x = x, y = y2 });
                dat.Add(new { id = "x*sin(x*x)", x = x, y = y3 });
            }
            return dat;
        }

        public static object MathData()
        {
            // 1/(x*x+1):
            double[] range = new double[] { -5.0, 5.0 };
            Func<double, double> f = (x) => 1.0 / (x * x + 1.0);
            var data1 = MathFunctionData(f, range, 300);

            // sin(x):
            range = new double[] { 0, 2.0 * Math.PI };
            f = (x) => Math.Sin(x);
            var data2 = MathFunctionData(f, range, 300);

            // sqrt(x):
            range = new double[] { 0, 10 };
            f = (x) => Math.Sqrt(x);
            var data3 = MathFunctionData(f, range, 300);

            // x*sin(x*x)+1:
            range = new double[] { -2.0, 3.0 };
            f = (x) => x * Math.Sin(x * x) + 1.0;
            var data4 = MathFunctionData(f, range, 300);

            return new { data1, data2, data3, data4 };
        }


        private static List<object> MathFunctionData(Func<double, double> f, double[] xRange, int numPoints)
        {
            double dx = (xRange[1] - xRange[0]) / numPoints;
            List<object> objs = new List<object>();
            for (var x = xRange[0]; x < xRange[1]; x = x + dx)
            {
                var y = f(x);
                objs.Add(new { x, y });
            }
            return objs;
        }

    }
}
