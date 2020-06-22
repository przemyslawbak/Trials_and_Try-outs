using System;
using System.Collections.Generic;

namespace BasicConfig.Models
{
    public class ModelHelper
    {
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
