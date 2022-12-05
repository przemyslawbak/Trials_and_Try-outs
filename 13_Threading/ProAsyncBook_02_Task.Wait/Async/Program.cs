using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3–3. Thread Pool Thread
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(Speak).Wait(); //typical for computebased tasks.
        }

        private static void Speak()
        {
            Console.WriteLine("Hello World");
        }
    }
}
