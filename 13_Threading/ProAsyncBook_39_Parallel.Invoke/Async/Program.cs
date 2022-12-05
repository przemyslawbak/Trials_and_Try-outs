using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 11-4. First Attmept at Canceling a Parallel.Invoke, with cancellation

    public class Program
    {
        static void Main(string[] args)
        {
            int sumX = 0;
            int sumY = 0;
            int sumZ = 0;

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(2000);

            try
            {
                // Executes SumX, SumY and SumZ in parallel
                Parallel.Invoke(new ParallelOptions() { CancellationToken = cts.Token },
                () => sumX = SumX(),
                () => sumY = SumY(),
                () => sumZ = SumZ()
                );

                // SumX,SumY and SumZ all complete
                int total = sumX + sumY + sumZ;

                Console.WriteLine(total);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cancelled: " + ex.Message);
            }

            Console.WriteLine("koniec");
            Console.ReadKey();
        }

        private static int SumZ()
        {
            //Thread.Sleep(10000);

            return 3;
        }

        private static int SumY()
        {
            //Thread.Sleep(5000);

            return 3;
        }

        private static int SumX()
        {
            //Thread.Sleep(5000);

            return 3;
        }
    }
}
