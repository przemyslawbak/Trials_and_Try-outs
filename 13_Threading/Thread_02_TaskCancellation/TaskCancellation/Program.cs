using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();
            Console.ReadKey();
        }

        private static void Initialize()
        {
            RunLoopedTasksAsync();
        }

        private static async void RunLoopedTasksAsync()
        {
            List<Task> tasks = new List<Task>();
            var _tokenSource = new CancellationTokenSource();
            CancellationToken _cancellationToken = _tokenSource.Token;

            //some loop
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(100);
                int j = i;
                var task = Task.Run(() =>
                {
                    Console.WriteLine(j);
                    // Were we already canceled?
                    _cancellationToken.ThrowIfCancellationRequested();
                }, _tokenSource.Token); // Pass same token to StartNew.

                tasks.Add(task);
                if (j == 50)
                {
                    _tokenSource.Cancel();
                }
            }


            // Just continue on this thread, or Wait/WaitAll with try-catch:
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task was cancelled");
            }
            finally
            {
                _tokenSource.Dispose();
            }

            Console.WriteLine("I should be written after task cancellation");

            Console.ReadKey();
        }
    }
}
