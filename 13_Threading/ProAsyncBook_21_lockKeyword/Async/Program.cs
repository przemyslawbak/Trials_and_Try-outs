using System.Threading;

namespace Async
{
    //Listing 4-9. Using the lock Keyword
    class Program
    {
        static void Main(string[] args)
        {
            //
        }
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
                //Failing to do this can result in hard-to-identify deadlocks and other synchronization bugs.
                //solution: Monitor.TryEnter
                lock (stateGuard) //lock: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/lock-statement
                {
                    cash += amount;
                    receivables -= amount;
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
