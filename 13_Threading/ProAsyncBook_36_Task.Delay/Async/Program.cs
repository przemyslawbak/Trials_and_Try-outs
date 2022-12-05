using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 7-12. Synchronous back off and retry

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
            await Task.Delay(1000);
        }
    }
}
