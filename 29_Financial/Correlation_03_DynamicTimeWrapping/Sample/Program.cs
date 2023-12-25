namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] dataA = new double[] { 0.1, 0.2, 0.2, 0.5, 0.9, 0.8, 0.7, 0.2, 0.4, 0.8, 0.1, 0.2 };
            double[] dataB = new double[] { 0.1, 0.2, 0.2, 0.5, 0.9, 0.8 };
            double[] dataC = new double[] { 0.1, 0.2, 0.2, 0.4, 0.9, 0.8, 0.7, 0.2, 0.4, 0.8, 0.1, 0.2 };
            double[] dataD = new double[] { 0.1, 0.0, 0.0, -0.3, -0.7, -0.6 };
            double[] dataE = new double[] { 2.1, 2.2, 2.2, 2.5, 2.9, 2.8, 2.7, 2.2, 2.4, 2.8, 2.1, 2.2 };
            double[] dataF = new double[] { 0.1, 0.1, 0.1, 0.1, 0.2, 0.2, 0.5, 0.9, 0.8, 0.7, 0.2, 0.4 };

            var correlationDtwB = FastDtw.CSharp.Dtw.GetScore(dataA, dataB);
            var correlationDtwC = FastDtw.CSharp.Dtw.GetScore(dataA, dataC);
            var correlationDtwD = FastDtw.CSharp.Dtw.GetScore(dataA, dataD);
            var correlationDtwE = FastDtw.CSharp.Dtw.GetScore(dataA, dataE);
            var correlationDtwF = FastDtw.CSharp.Dtw.GetScore(dataA, dataF);
        }
    }
}