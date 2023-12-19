using System.Collections.Concurrent;

namespace Activator
{
    class Program
    {
        //https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?redirectedfrom=MSDN
        static void Main(string[] args)
        {
            // Static partitioning requires indexable source. Load balancing
            // can use any IEnumerable.
            var nums = Enumerable.Range(0, 5000).ToArray();

            var watchSync = System.Diagnostics.Stopwatch.StartNew();

            foreach (var i in nums)
            {
                var l = ProcessData(i);
            }

            watchSync.Stop();
            var edMs = watchSync.ElapsedMilliseconds;
            Console.WriteLine("sync (ms): " + edMs);

            Console.ReadKey();

            var watchParallel = System.Diagnostics.Stopwatch.StartNew();

            // Create a load-balancing partitioner. Or specify false for static partitioning.
            Partitioner<int> customPartitioner = Partitioner.Create(nums, true);

            // The partitioner is the query's data source.
            var q = customPartitioner.AsParallel().Select(x => x);

            q.ForAll(x =>
            {
                var l = ProcessData(x);
            });

            watchParallel.Stop();
            var elapsedMs = watchParallel.ElapsedMilliseconds;
            Console.WriteLine("parallel (ms): " + elapsedMs);
        }

        private static long ProcessData(int n)
        {
            Console.WriteLine(n);
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }
    }
}



