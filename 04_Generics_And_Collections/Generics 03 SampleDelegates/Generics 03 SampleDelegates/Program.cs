using System;

namespace Generics_03_SampleDelegates
{
    // Definicja delegata
    delegate int NumberChangeNormalDef(int i);
    // Definicja generycznego delegate
    delegate T NumberChange<T>(T i);
    // Statyczna klasa testowa z metodami do dodawania, mnożenia oraz zwracania liczby
    static class Test
    {
        static int num = 10;
        public static int AddNumber(int a)
        {
            num += a;
            return num;
        }
        public static int MultiplyNumber(int m)
        {
            num *= m;
            return num;
        }
        public static int GetNumber()
        {
            return num;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Definicja delegata - dla porównania
            NumberChangeNormalDef normal = new NumberChangeNormalDef(Test.AddNumber);
            // Deklaracja instancji generycznych delegatów
            NumberChange<int> d1 = new NumberChange<int>(Test.AddNumber);
            NumberChange<int> d2 = new NumberChange<int>(Test.MultiplyNumber);
            // Wywołanie metod używając obiektu delegata
            d1(5);
            Console.WriteLine("Liczba: {0}", Test.GetNumber());
            d2(10);
            Console.WriteLine("Liczba: {0}", Test.GetNumber());
            Console.ReadKey();
            // Wynik działania programu
            // Liczba: 15
            // Liczba: 150
        }
    }
}
