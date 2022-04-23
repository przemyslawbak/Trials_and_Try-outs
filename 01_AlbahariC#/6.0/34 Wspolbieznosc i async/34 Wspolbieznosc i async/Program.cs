using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _34_Wspolbieznosc_i_async
{
    class Program
    {
        static bool _done; // statyczne elementy składowe są współdzielone między
                           // wszystkimi wątkami w tej samej domenie aplikacji
        static readonly object _locker = new object(); //Nakładanie blokad i zapewnianie bezpieczeństwa wątków
        static void Main(string[] args)
        {
            //przykład wywłaszczenia
            Thread t = new Thread(WriteY); // utworzenie nowego wątku
            t.Start(); // wykonanie metody WriteY()
                       // jednocześnie wykonujemy operacje w wątku głównym
            for (int i = 0; i < 1000; i++) Console.Write("x");

            //join - Istnieje możliwość zaczekania, aż inny wątek zakończy pracę. W tym celu należy wywołać jego metodę Join().
            t = new Thread(Go);
            t.Start(); //pisanie "y" start
            t.Join(); //poczekaliśmy aż "y" się napisze
            Console.WriteLine("Wątek t został zakończony!");

            //Statyczne elementy składowe to kolejny sposób pozwalający na współdzielenie danych między wątkami
            new Thread(Goo).Start(); // wywołanie metody Go() w nowo utworzonym wątku
            Goo();

            //przekazywanie danych do wątku
            t = new Thread(Print);
            t.Start("Pozdrowienia z t!");
            new Thread(() =>
            {
                Console.WriteLine("Działam w innym wątku!");
                Console.WriteLine("To jest takie łatwe!");
            }).Start();

            //exceptions
            try
            {
                new Thread(Gow).Start();
            }
            catch (Exception ex)
            {
                // Nigdy nie dotrzemy do tego miejsca!
                Console.WriteLine("Wyjątek!");
            }

            //zadania
            Task.Run(() => Console.WriteLine("Foo"));

            //wyjątki
            Task task = Task.Run(() => { throw null; });
            try
            {
                task.Wait();
            }
            catch (AggregateException aex)
            {
                if (aex.InnerException is NullReferenceException)
                    Console.WriteLine("Null!");
                else
                    throw;
            }

            //kontynuacja
            Task<int> primeNumberTask = Task.Run(() =>
            Enumerable.Range(2, 3000000).Count(n =>
            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
            var awaiter = primeNumberTask.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                Console.WriteLine(result); // zapis wyniku
            });
            Console.ReadKey();
        }
        static void WriteY()
        {
            for (int i = 0; i < 1000; i++) Console.Write("y");
        }
        static void Go() { for (int i = 0; i < 1000; i++) Console.Write("y"); }
        static void Goo()
        {
            lock (_locker) //nałożenie blokady wykluczającej
            {
                if (!_done) { Console.WriteLine("Gotowe"); _done = true; }
            }
        }
        static void Print(object messageObj)
        {
            string message = (string)messageObj; // w tym miejscu trzeba zastosować rzutowanie
            Console.WriteLine(message);
        }
        static void Gow() { throw null; } // zgłoszenie wyjątku NullReferenceException
    }
}
