using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-10. Working Capture
    class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                int iterator = i; //bez tego wywali same 10
                Task.Factory.StartNew(() => Console.WriteLine(iterator));
            }

            Console.ReadKey();

            /*
             * in the case of foreach, the compiler team has moved the point of capture to inside the loop automatically
             */
            foreach (int j in new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            {
                Task.Factory.StartNew(() => Console.WriteLine(j));
            }

            Console.ReadKey();
        }
    }
}
