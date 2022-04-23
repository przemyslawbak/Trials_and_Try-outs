using System;
using System.Text;

namespace _07_Zmienne_i_parametry
{
    class Program
    {
        static void Swap(ref string a, ref string b)
        {
            string temp = a;
            a = b;
            b = temp;
        }
        static void Foo(StringBuilder fooSB)
        {
            fooSB.Append("test");
            fooSB = null;
        }
        static void Foo(int p)
        {
            p = p + 1; // zwiększenie wartości o jeden
            Console.WriteLine(p); // drukuje wartość p na ekranie
        }
        static int x;
        static int Factorial(int x)
        {
            if (x == 0) return 1;
            return x * Factorial(x - 1);
        }
        static void Foo(ref int p)
        {
            p = p + 1; // zwiększenie wartości o 1
            Console.WriteLine(p); // drukuje wartość p na ekranie
        }
        static void Split(string name, out string firstNames, out string lastName)
        {
            int i = name.LastIndexOf(' ');
            firstNames = name.Substring(0, i);
            lastName = name.Substring(i + 1);
        }
        static int Sum(params int[] ints)
        {
            int sum = 0;
            for (int i = 0; i < ints.Length; i++)
                sum += ints[i]; // zwiększenie sumy o ints[i]
            return sum;
        }
        static void Main(string[] args)
        {
            StringBuilder ref1 = new StringBuilder("obiekt1");
            Console.WriteLine(ref1);
            // obiekt typu StringBuilder wskazywany przez zmienną ref1 można już przekazać do systemu usuwania nieużytków
            StringBuilder ref2 = new StringBuilder("obiekt2");
            StringBuilder ref3 = ref2;
            // obiekt StringBuilder wskazywany przez zmienną ref2 jeszcze nie jest gotowy do przekazania systemowi usuwania nieużytków
            Console.WriteLine(ref3); // obiekt2

            int[] ints = new int[2];
            Console.WriteLine(ints[0]); // 0

            Foo(8); // wywołanie Foo z argumentem 8

            int x = 8;
            Foo(x); // utworzenie kopii x
            Console.WriteLine(x); // x nadal będzie mieć wartość 8

            StringBuilder sb = new StringBuilder();
            Foo(sb);
            Console.WriteLine(sb.ToString()); // test

            int y = 8;
            Foo(ref y); // nakazujemy Foo pracować bezpośrednio z x
            Console.WriteLine(y); // x ma teraz wartość 9

            string r = "Penn";
            string t = "Teller";
            Swap(ref r, ref t);
            Console.WriteLine(r); // Teller
            Console.WriteLine(t); // Penn

            string c, d;
            Split("Stevie Ray Vaughan", out c, out d);
            Console.WriteLine(c); // Stevie Ray
            Console.WriteLine(d); // Vaughan

            int total = Sum(new int[] { 1, 2, 3, 4 });
            Console.WriteLine(total);


            int rr = Sum(1, 2, 3, 4);
            Console.WriteLine(rr); // 10

            Console.ReadKey();
        }
    }

}
