using System;
using System.Threading;

namespace Async
{
    //Listing 4-11/12. Wrapping up Monitor.TryEnter
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
                using (stateGuard.Lock(TimeSpan.FromSeconds(30)))
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

            public static class LockExtensions
            {
                public static LockHelper Lock(this object obj, TimeSpan timeout)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(obj, TimeSpan.FromSeconds(30), ref lockTaken);
                        if (lockTaken)
                        {
                            return new LockHelper(obj);
                        }
                        else
                        {
                            throw new TimeoutException("Failed to acquire stateGuard");
                        }
                    }
                    catch
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(obj);
                        }
                        throw;
                    }
                }
                private struct LockHelper : IDisposable
                {
                    private readonly object obj;
                    public LockHelper(object obj)
                    {
                        this.obj = obj;
                    }
                    public void Dispose()
                    {
                        Monitor.Exit(obj);
                    }
                }
            }
        }
    }
}
