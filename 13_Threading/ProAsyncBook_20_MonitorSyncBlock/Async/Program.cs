using System.Threading;

namespace Async
{
    //Listing 4-6. Thread-Safe SmallBusiness
    class Program
    {
        static void Main(string[] args)
        {
            //
        }

        /// <summary>
        /// z książki:
        /// If a thread, A, calls Monitor.Enter and another thread, B, already owns the specified monitor, then Monitor.Enter
        /// blocks until thread B calls Monitor.Exit.
        /// </summary>
        public class SmallBusiness
        {
            private decimal cash;
            private decimal receivables;
            private readonly object stateGuard = new object();
            public SmallBusiness(decimal cash, decimal receivables)
            {
                this.cash = cash;
                this.receivables = receivables;
            }

            public void ReceivePayment(decimal amount)
            {
                bool lockTaken = false;
                Monitor.Enter(stateGuard);
                try
                {
                    Monitor.Enter(stateGuard, ref lockTaken); //ref: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#passing-an-argument-by-reference
                    cash += amount;
                    receivables -= amount;
                }
                finally //na wypadek wyjątku
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(stateGuard);
                    }
                }
            }

            public decimal NetWorth
            {
                get
                {
                    Monitor.Enter(stateGuard);
                    decimal netWorth = cash + receivables;
                    Monitor.Exit(stateGuard);
                    return netWorth;
                }
            }
        }
}
