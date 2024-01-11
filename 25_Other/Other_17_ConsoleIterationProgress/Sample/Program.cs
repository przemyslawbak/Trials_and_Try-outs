using System.Collections.Concurrent;

namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            var percentage = 0.00;
            var progress = 0;
            Console.WriteLine("Progress:");
            for (int i = 0; i < 654; i++)
            {
                progress++;
                Console.Write("\r{0}%", percentage);
                Thread.Sleep(100);
                percentage = Math.Round((double)progress / 654 * 100, 2);
            }
            Console.WriteLine();
            Console.WriteLine("Finito.");
        }
    }
}



