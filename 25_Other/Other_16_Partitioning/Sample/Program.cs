using System.Collections.Concurrent;

namespace Activator
{
    class Program
    {
        //https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?redirectedfrom=MSDN
        //optional: https://stackoverflow.com/a/22273528
        static void Main(string[] args)
        {
            // Static partitioning requires indexable source. Load balancing
            // can use any IEnumerable.
            var nums = Enumerable.Range(0, 5000).ToArray();

            var watchSync = System.Diagnostics.Stopwatch.StartNew();
            List<long> syncList = new List<long>();

            foreach (var i in nums)
            {
                var l = ProcessData(i);
                syncList.Add(l);
            }

            watchSync.Stop();
            var edMs = watchSync.ElapsedMilliseconds;
            Console.WriteLine("sync (ms): " + edMs);
            Console.WriteLine("list count: " + syncList.Count);

            Console.ReadKey();

            var watchParallel = System.Diagnostics.Stopwatch.StartNew();

            // Create a load-balancing partitioner. Or specify false for static partitioning.
            Partitioner<int> customPartitioner = Partitioner.Create(nums, true);

            // The partitioner is the query's data source.
            var q = customPartitioner.AsParallel().Select(x => x);

            List<long> paraList = new List<long>();

            q.ForAll(x =>
            {
                var l = ProcessData(x);
                lock (paraList)
                {
                    paraList.Add(l);
                }
            });

            watchParallel.Stop();
            var elapsedMs = watchParallel.ElapsedMilliseconds;
            Console.WriteLine("parallel (ms): " + elapsedMs);
            Console.WriteLine("list count: " + paraList.Count);
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



