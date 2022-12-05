using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 11-10. Parallel.For and Associated Tasks

    public class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 20, i =>
            {
                Console.WriteLine("{0} : {1}", Task.CurrentId, i);
                Thread.SpinWait(100000000);
            });
        }
    }
}
