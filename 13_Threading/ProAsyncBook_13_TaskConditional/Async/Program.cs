using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-34. Two-Conditional Continuation
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> firstTask = Task.Factory.StartNew<int>(() => { Console.WriteLine("First Task"); return 42; });

            Task secondTask = firstTask.ContinueWith(ProcessResult, TaskContinuationOptions.OnlyOnRanToCompletion); //wynik albo wyjątek
            Task errorHandler = firstTask.ContinueWith(st => Console.WriteLine(st.Exception),
            TaskContinuationOptions.OnlyOnFaulted);

            secondTask.Wait();
        }

        private static void ProcessResult(Task<int> arg1, object arg2)
        {
            throw new NotImplementedException();
        }
    }
}
