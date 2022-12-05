using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-24/25/26.
    class Program
    {
        static void Main(string[] args)
        {

        }

        public static void DataImport(IImport import)
        {
            var tcs = new CancellationTokenSource();
            CancellationToken ct = tcs.Token;

            Task importTask = import.ImportXmlFilesAsync(@"C:\data", ct);

            while (!importTask.IsCompleted)
            {
                Console.Write(".");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    tcs.Cancel();
                }
                Thread.Sleep(250);
            }
        }
    }

    public interface IImport
    {
        Task ImportXmlFilesAsync(string dataDirectory);
        Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct);
    }
}
