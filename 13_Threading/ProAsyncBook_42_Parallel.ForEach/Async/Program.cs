using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Async
{
    //Listing 11-17. Simple Parallel.ForEach

    public class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> items = Enumerable.Range(0, 20000000);
            Parallel.ForEach(items, i =>
            {
                for (int j = 0; j < 10_000_000; j++)
                {
                    double v = Math.Tan(j);
                }
            });
        }
    }
}
