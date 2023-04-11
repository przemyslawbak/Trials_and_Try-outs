using System;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        private static Hidden _hiddenService;
        static void Main(string[] args)
        {
            _hiddenService = new Hidden();
            DoSomething().Wait();
        }

        private static async Task DoSomething()
        {
            string k = _hiddenService.GetK();
        }
    }
}
