using MathNet.Numerics.Statistics;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] dataA = new double[] { 0.1, 0.2, 0.2, 0.5, 0.9, 0.8 };
            double[] dataB = new double[] { 0.1, 0.2, 0.2, 0.5, 0.9, 0.8 };
            double[] dataC = new double[] { 0.1, 0.2, 0.2, 0.4, 0.9, 0.8 };
            double[] dataD = new double[] { 0.1, 0.0, 0.0, -0.3, -0.7, -0.6 };
            double[] dataE = new double[] { 2.1, 2.2, 2.2, 2.5, 2.9, 2.8 };

            var correlationSpearmanB = Correlation.Spearman(dataA, dataB);
            var correlationSpearmanC = Correlation.Spearman(dataA, dataC);
            var correlationSpearmanD = Correlation.Pearson(dataA, dataD);
            var correlationSpearmanE = Correlation.Pearson(dataA, dataE);
        }
    }
}
