using System;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<int> list = GenerateList();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ShuffleIntList(list);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("finito1: " + elapsedMs + "ms");

            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            ShuffleIntList2(list);
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;

            Console.WriteLine("finito2: " + elapsedMs2 + "ms");

            Console.Read();
        }

        private static List<int> GenerateList()
        {
            List<int> l = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                l.Add(i);
            }

            return l;
        }

        private static void ShuffleIntList(List<int> l)
        {
            int tmp;
            int randIdx;

            for (int i = 0; i < l.Count; i++)
            {
                Random r = new Random();
                randIdx = r.Next(0, l.Count);
                tmp = l[i];

                l[i] = l[randIdx];
                l[randIdx] = tmp;
            }
        }

        private static void ShuffleIntList2(List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                Random r = new Random();
                int randIdx = r.Next(0, l.Count);
                int tmp = l[i];

                l[i] = l[randIdx];
                l[randIdx] = tmp;
            }
        }
    }
}
