using System;
using System.Collections.Generic;
using System.Linq;

namespace ToList
{
    class Program
    {
        //https://stackoverflow.com/a/4460167
        static void Main(string[] args)
        {
            List<int> A = new List<int>() { 1,2,3,4,5,5,6,7,8,8,9,9,0 };

            var m = Enumerable.Range(1, A.Count - 1)
                  .Select(i => A[i] == A[i - 1])
                  .Where(x => x == true)
                  .Count();

            Console.ReadKey();
        }
    }
}
