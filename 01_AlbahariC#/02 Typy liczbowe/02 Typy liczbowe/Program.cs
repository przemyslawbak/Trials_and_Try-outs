using System;

namespace _02_Typy_liczbowe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nInferencja typów literałów liczbowych");
            Console.WriteLine(1.0.GetType()); // Double (double)
            Console.WriteLine(1E06.GetType()); // Double (double)
            Console.WriteLine(1.GetType()); // Int32 (int)
            Console.WriteLine(0xF0000000.GetType()); // UInt32 (uint)
            Console.WriteLine(0x100000000.GetType()); // Int64 (long)
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nKonwersje liczbowe");
            int x = 12345; // int to 32-bitowy typ całkowitoliczbowy
            long y = x; // niejawna konwersja na 64-bitowy typ całkowitoliczbowy
            short z = (short)x; // jawna konwersja na 16-bitowy typ całkowitoliczbowy
            int i1 = 100000001;
            float f = i1; // rząd wielkości zachowany, utracona precyzja
            int i2 = (int)f; // 100000000
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nOperatory inkrementacji i dekrementacji");
            int a = 0, b = 0;
            Console.WriteLine(a++); // wynik 0; a ma wartość 1
            Console.WriteLine(++b); // wynik 1; b ma wartość 1
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nDzielenie całkowitoliczbowe");
            int c = 2 / 3; // 0
            int d = 0;
            //int e = 5 / d; // błąd DivideByZeroException
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nPrzepełnienie całkowite");
            int g = int.MinValue;
            g--;
            Console.WriteLine(g == int.MaxValue); // prawda
            Console.WriteLine(g); //wartość
            //dla automatyczne sprawdzania
            int h = int.MaxValue;
            int i = unchecked(h + 1);
            unchecked { int j = h + 1; }
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nZaokrąglanie");
            decimal m = 1M / 6M; // 0.1666666666666666666666666667M
            double n = 1.0 / 6.0; // 0.16666666666666666
            decimal notQuiteWholeM = m + m + m + m + m + m; // 1.0000000000000000000000000002M
            double notQuiteWholeD = n + n + n + n + n + n; // 0.99999999999999989
            Console.WriteLine(notQuiteWholeM == 1M); // fałsz
            Console.WriteLine(notQuiteWholeD < 1.0); // prawda
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }
    }
}
