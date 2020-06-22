using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Models
{
    public static class LinearAnalysisHelper
    {
        public static SimpleLinearResult GetSimpleRegression<T>(ObservableCollection<T> collection, string xField, string yField)
        {
            List<double> xl = new List<double>();
            List<double> yl = new List<double>();
            foreach (var p in collection)
            {
                double x = (double)p.GetType().GetProperty(xField).GetValue(p);
                double y = (double)p.GetType().GetProperty(yField).GetValue(p);
                xl.Add(x);
                yl.Add(y);
            }

            return GetSimpleRegression(xl, yl);
        }


        public static SimpleLinearResult GetSimpleRegression(double[] xData, double[] yData)
        {
            List<double> xl = new List<double>();
            List<double> yl = new List<double>();
            xl.AddRange(xData);
            yl.AddRange(yData);
            return GetSimpleRegression(xl, yl);
        }

        public static SimpleLinearResult GetSimpleRegression(List<double> xData, List<double> yData)
        {
            double xa = xData.Average();
            double ya = yData.Average();

            double a1 = 0.0;
            double a2 = 0.0;
            for (int i = 0; i < xData.Count; i++)
            {
                a1 += yData[i] * (xData[i] - xa);
                a2 += xData[i] * (xData[i] - xa);
            }

            //get coefficients a, b in y = a + b *x:
            double a = 0.0;
            double b = 0.0;
            if (Math.Abs(a2) > 0)
                b = a1 / a2;
            a = ya - b * xa;
            double r2 = GetRSquared(xData, yData, a, b);
            double r2Adj = GetRSquaredAdj(r2, 1, xData.Count);
            return new SimpleLinearResult { Alpha = a, Beta = b, RSquared = r2, RSquaredAdj = r2Adj };
        }


        public static SimpleLinearResult GetSimplePca<T>(ObservableCollection<T> collection, string xField, string yField)
        {
            List<double> xl = new List<double>();
            List<double> yl = new List<double>();
            foreach (var p in collection)
            {
                double x = (double)p.GetType().GetProperty(xField).GetValue(p);
                double y = (double)p.GetType().GetProperty(yField).GetValue(p);
                xl.Add(x);
                yl.Add(y);
            }

            return GetSimplePca(xl, yl);
        }

        public static SimpleLinearResult GetSimplePca(double[] xData, double[] yData)
        {
            List<double> xl = new List<double>();
            List<double> yl = new List<double>();
            xl.AddRange(xData);
            yl.AddRange(yData);
            return GetSimplePca(xl, yl);
        }

        public static SimpleLinearResult GetSimplePca(List<double> xData, List<double> yData)
        {
            double sumx = 0.0;
            double sumy = 0.0;
            double sumxx = 0.0;
            double sumyy = 0.0;
            double sumxy = 0.0;
            int n = xData.Count;

            //substrate mean first:
            double xa = xData.Average();
            double ya = yData.Average();

            List<double> xData1 = new List<double>();
            List<double> yData1 = new List<double>();
            foreach (var p in xData)
                xData1.Add(p);
            foreach (var p in yData)
                yData1.Add(p);

            for (int i = 0; i < n; i++)
            {
                xData1[i] = xData[i] - xa;
                yData1[i] = yData[i] - ya;
            }

            for (int i = 0; i < n; i++)
            {
                sumx += xData1[i];
                sumy += yData1[i];
                sumxx += xData1[i] * xData1[i];
                sumyy += yData1[i] * yData1[i];
                sumxy += xData1[i] * yData1[i];
            }

            //variance and covariance
            double a1 = sumxx / (n - 1);
            double c1 = sumyy / (n - 1);
            double b1 = sumxy / (n - 1);

            //eigen values:
            double d = Math.Sqrt((a1 - c1) * (a1 - c1) + 4.0 * b1 * b1);
            double lp = 0.5 * (a1 + c1 + d);
            double lm = 0.5 * (a1 + c1 - d);

            //eigen vectors:
            double d1 = Math.Sqrt(2.0 * d);
            double dp = Math.Sqrt(d + (c1 - a1));
            double dm = Math.Sqrt(d - (c1 - a1));
            double ap = 2.0 * b1 / d1 / dp;
            double bp = (d + (c1 - a1)) / d1 / dp;
            double am = -2.0 * b1 / d1 / dm;
            double bm = (d - (c1 - a1)) / d1 / dm;

            //choose pca vector:
            double lMax = Math.Max(lp, lm);
            double[] ab = new double[] { ap, bp };
            if (lMax == lm)
                ab = new double[] { am, bm };

            //get coefficients a, b in y = a + b *x:
            double a = 0.0;
            double b = 0.0;
            if (Math.Abs(ab[0]) > 0)
                b = ab[1] / ab[0];
            a = ya - b * xa;

            double r2 = GetRSquared(xData, yData, a, b);
            double r2Adj = GetRSquaredAdj(r2, 1, xData.Count);
            return new SimpleLinearResult { Alpha = a, Beta = b, RSquared = r2, RSquaredAdj = r2Adj };
        }

        public static double GetRSquaredAdj(double r2, int p, int n)
        {
            return r2 - (1.0 - r2) * p / (n - p - 1.0);
        }

        public static double GetRSquared(List<double> xData, List<double> yData, double alpha, double beta)
        {
            List<double> fData = GenerateData(xData, alpha, beta);
            double avg = yData.Average();
            double ss_tot = 0.0;
            double ss_res = 0.0;

            for (int i = 0; i < xData.Count; i++)
            {
                double f = fData[i];
                double y = yData[i];
                ss_tot += (y - avg) * (y - avg);
                ss_res += (y - f) * (y - f);
            }
            return 1.0 - ss_res / ss_tot;
        }


        public static List<double> GenerateData(List<double> xl, double alpha, double beta)
        {
            List<double> yList = new List<double>();
            for (int i = 0; i < xl.Count; i++)
            {
                double y = alpha + beta * xl[i];
                yList.Add(y);
            }
            return yList;
        }

        public static List<double> GenerateData(SimpleLinearResult res, List<double> xl)
        {
            List<double> yList = new List<double>();
            for (int i = 0; i < xl.Count; i++)
            {
                double y = res.Alpha + res.Beta * xl[i];
                yList.Add(y);
            }
            return yList;
        }
    }

    public class SimpleLinearResult
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double RSquared { get; set; }
        public double RSquaredAdj { get; set; }
    }
}
