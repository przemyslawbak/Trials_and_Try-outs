using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-1. Asynchronous Hello World
    class Program
    {
        static void Main(string[] args)
        {
            Task t = new Task(Speak);
            t.Start();
            Console.ReadKey(); //bez tego główny wątek się zakończy zarówno jak i proces
            /*
             * By the time the task is about to run, the main thread has already terminated and, hence, the process will
                terminate as well.
            */
        }

        private static void Speak()
        {
            Console.WriteLine("Hello World");
        }
    }
}
