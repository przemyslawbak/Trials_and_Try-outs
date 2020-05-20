using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-12. ACME Lottery async
    class Program
    {
        private static void Main(string[] args)
        {
            int n = 49000;
            int r = 600;
            Task<int> part1 = Task.Factory.StartNew<int>(() => FactorialPart1(n));
            Task<int> part2 = Task.Factory.StartNew<int>(() => FactorialPart2(n - r));
            Task<int> part3 = Task.Factory.StartNew<int>(() => FactorialPart3(r));

            int chances = part1.Result / (part2.Result * part3.Result);
            Console.WriteLine(chances);
        }

        private static int FactorialPart3(int r)
        {
            return 1;
        }

        private static int FactorialPart2(int v)
        {
            return 10;
        }

        private static int FactorialPart1(int n)
        {
            return 100;
        }
    }
}
