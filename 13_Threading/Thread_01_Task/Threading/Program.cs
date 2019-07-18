using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteAsync();



            Console.ReadLine();
        }

        private static async void ExecuteAsync()
        {
            Program program = new Program();
            await program.SomeLoop();
        }


        private async Task SomeLoop()
        {
            for (int i = 0; i < 40; ++i)
                await Task.Run(() => Console.WriteLine(i));

            Console.WriteLine("Finished. Press <ENTER> to exit.");
            Console.ReadLine();
        }
    }
}
