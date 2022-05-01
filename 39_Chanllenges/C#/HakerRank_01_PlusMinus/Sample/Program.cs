using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> arr = new List<int>()
            {
                -4, 3, -9, 0, 4, 1
            };

            var plus = arr.Where(a => a > 0);
            var minus = arr.Where(a => a < 0);
            var zero = arr.Where(a => a == 0);
            Console.WriteLine(((double)plus.Count() / arr.Count()).ToString("0.000000"));
            Console.WriteLine(((double)minus.Count() / arr.Count()).ToString("0.000000"));
            Console.WriteLine(((double)zero.Count() / arr.Count()).ToString("0.000000"));
        }
    }
}
