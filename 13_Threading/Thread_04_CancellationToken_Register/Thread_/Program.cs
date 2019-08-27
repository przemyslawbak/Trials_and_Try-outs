using System;
using System.Threading;

namespace Thread_
{
    //Registers a delegate that will be called when this System.Threading.CancellationToken
    //https://stackoverflow.com/a/19317884
    class Program
    {
        static void Main(string[] args)
        {
            Cts = new CancellationTokenSource();

            Ct = Cts.Token;

            RegisterFirst(Ct);

            RegisterSecond(Ct);

            Console.ReadKey();

            Cts.Cancel();

            Console.WriteLine("cancelled...");

            Console.ReadLine();
        }

        static public CancellationTokenSource Cts { get; set; }
        static public CancellationToken Ct { get; set; }

        static void RegisterFirst(CancellationToken token)
        {
            token.Register(() => Console.WriteLine("Registered first"));
        }

        static void RegisterSecond(CancellationToken token)
        {
            token.Register(() => Console.WriteLine("Registered second"));
        }
    }
}
