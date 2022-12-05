using System.Threading;

namespace Async
{
    //Listing 4-2. Using Interlocked.Increment
    class Program
    {
        static void Main(string[] args)
        {
            //
        }

        //These methods turn their nonatomic counterparts into atomic operations
        private static void IncrementValue(ref int value, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Interlocked.Increment(ref value); //Interlocked.Increment is cheap if there is no contention with another thread, but becomes more expensive if there is.
            }
        }
    }
}
