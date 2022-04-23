using System;

namespace _11_Delegat
{
    delegate int Transformer(int x); //delegat

    public delegate void ProgressReporter(int percentComplete); //dla multicast

    public interface ITransformer //zamiast delegatu
    {
        int Transform(int x);
    }
    class Util
    {
        public static void TransformAll(int[] values, ITransformer t)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = t.Transform(values[i]);
        }
    }
    class Squarer : ITransformer
    {
        public int Transform(int x) => x * x;
    }
    public class Util1 //dla multicast
    {
        public static void HardWork(ProgressReporter p)
        {
            for (int i = 0; i < 10; i++)
            {
                p(i * 10); // wywołanie delegatu
                System.Threading.Thread.Sleep(100); // symulacja ciężkiej pracy
            }
        }
    }
    class Program
    {
        static int Square(int x) => x * x;
        static void Main(string[] args)
        {
            int[] values = { 1, 2, 3 };
            Util.TransformAll(values, new Squarer());
            foreach (int i in values)
                Console.WriteLine(i);
            Console.ReadKey();

            //multicast
            ProgressReporter p = WriteProgressToConsole;
            p += WriteProgressToFile;
            Util1.HardWork(p);
        }
        //multicast
        static void WriteProgressToConsole(int percentComplete) => Console.WriteLine(percentComplete);
        static void WriteProgressToFile(int percentComplete)
        => System.IO.File.WriteAllText("progress.txt",
        percentComplete.ToString());
    }
}
