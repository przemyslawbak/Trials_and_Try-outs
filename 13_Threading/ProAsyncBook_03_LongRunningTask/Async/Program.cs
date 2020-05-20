using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3–4. Long-Running Task
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory
            .StartNew(Speak, TaskCreationOptions.LongRunning)
            .Wait();
        }

        private static void Speak()
        {
            Console.WriteLine("Hello World");
        }
    }
}
