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
            IEnumerable<int> items = Enumerable.Range(0, 20);
            Parallel.ForEach(items, i =>
            {
                Console.WriteLine(i);
            });
        }
    }
}
