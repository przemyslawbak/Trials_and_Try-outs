using System;

namespace Generics_02_SampleMethod
{
    class Program
    {
        // Generyczna metoda to zamiany kolejności parametrów
        static void Swap<T>(ref T a, ref T b)
        {
            T temp;
            temp = a;
            a = b;
            b = temp;
        }
        static void Main(string[] args)
        {
            int a, b;
            char c, d;
            a = 10;
            b = 20;
            c = 'A';
            d = 'B';
            // Wartości przed zamianą
            Console.WriteLine("a = {0}, b = {1}", a, b);
            Swap(ref a, ref b);
            // Wartości po zmianie
            Console.WriteLine("a = {0}, b = {1}", a, b);
            // Sprawdzmy teraz innych typ danych
            // Wartości przed zamianą
            Console.WriteLine("a = {0}, b = {1}", c, d);
            Swap(ref c, ref d);
            // Wartości po zmianie
            Console.WriteLine("a = {0}, b = {1}", c, d);
            Console.ReadKey();
            // Wynik działania programu
            // a = 10, b = 20
            // a = 20, b = 10
            // a = A, b = B
            // a = B, b = A
        }
    }
}
