using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-33. Simple Unconditional Continuation
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> firstTask = Task.Factory.StartNew<int>(() => { Console.WriteLine("First Task"); return 42; });

            //The ContinueWith method creates the second task, which will be activated once the firstTask has completed.
            Task secondTask = firstTask.ContinueWith(ft => Console.WriteLine("Second Task, First task returned {0}", ft.Result));

            secondTask.Wait();
        }
    }
}
