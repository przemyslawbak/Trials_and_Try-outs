using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 7-16. Observing WhenAll task exceptions

    public class Program
    {
        static void Main(string[] args)
        {
            for (int nTry = 0; nTry < 3; nTry++)
            {
                try
                {
                    AttemptOperation();
                    break;
                }
                catch (Exception ex) { }
            }
        }

        private static async void AttemptOperation()
        {
            List<Task> tasks = new List<Task>();
            Task allDownloads = Task.WhenAll(tasks);
            try
            {
                await allDownloads;
                tasks.ForEach(t => UpdateUI(t.Result));
            }
            catch (Exception e)
            {
                allDownloads.Exception.Handle(exception =>
                {
                    Console.WriteLine(exception.Message);
                    return true;
                });
            }
        }
    }
}
