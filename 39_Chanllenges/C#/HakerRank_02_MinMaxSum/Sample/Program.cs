using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<uint> arr = new List<uint>()
            {
                256741038, 623958417, 467905213, 714532089, 938071625
            };

            List<ulong> results = new List<ulong>();

            for (int i = 0; i < arr.Count(); i++)
            {
                List<uint> list = new List<uint>(arr);
                list.RemoveAt(i);
                ulong sum = (ulong)list.Sum(x => x);
                results.Add(sum);
            }

            ulong max = results.Max(r => r);
            ulong min = results.Min(r => r);

            Console.WriteLine(min + " " + max);
        }
    }
}
