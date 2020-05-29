using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 11-16. Parallel Pi with Thread-Safe Combiner

    public class Program
    {
        static void Main(string[] args)
        {
            double pi = ParallelCalculatePi(10);

            Console.ReadKey();
        }

        private static double ParallelCalculatePi(int iterations)
        {
            double pi = 1;
            object combineLock = new object();
            Parallel.For(0, (iterations - 3) / 2,
            InitialiseLocalPi,
            (int loopIndex, ParallelLoopState loopState, double localPi) =>
            {
                double multiplier = loopIndex % 2 == 0 ? -1 : 1;
                int i = 3 + loopIndex * 2;
                localPi += 1.0 / (double)i * multiplier;

                return localPi;
            },
            (double localPi) =>
            {
                lock (combineLock)
                {
                    pi += localPi;
                }
            });

            return pi * 4.0;
        }

        private static double InitialiseLocalPi()
        {
            return 0.0;
        }
    }
}
