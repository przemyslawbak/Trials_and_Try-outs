using System;
using System.Threading.Tasks;

namespace Testy_09_Async_Run
{
    class Program
    {
        public async Task<string> GetString()
        {
            string a = "output2";
            await Task.Run(() => {
                Task.Delay(1000);
                string k = "output";
                return k;
            });
            return a;
        }

        public async Task ShowString()
        {
            await Task.Run(() => Console.WriteLine(GetString() + "METODA")); //tu zwroci System.Threading.Tasks.Task`1[System.String]
            Console.WriteLine("hit key a");
            Console.ReadKey();
            
            await GetString();
            Console.WriteLine("hit key b");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            Program przyklad = new Program();
            przyklad.ShowString();
            Console.WriteLine(przyklad.GetString() + "RUN");
            Console.WriteLine("koniec");
            Console.ReadKey();
        }
    }
}
