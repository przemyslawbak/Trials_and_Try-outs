using System;
using System.Collections.Generic;

namespace _15_Wyliczenie_i_iteratory
{
    class Program
    {
        static void Main(string[] args)
        {
            //Można użyć wielu instrukcji yield
            foreach (string s in Foo())
                Console.WriteLine(s); // drukuje "Jeden","Dwa","Trzy"

            foreach (int fib in Fibs(6))
                Console.Write(fib + " ");
            //wysokopoziomowy przykład iteracji przez znaki słowa piwo za pomocą instrukcji foreach
            foreach (char c in "piwo")
                Console.WriteLine(c);
            //niskopoziomowy przykład iteracji przez znaki słowa piwo
            using (var enumerator = "piwo".GetEnumerator())
                while (enumerator.MoveNext())
                {
                    var element = enumerator.Current;
                    Console.WriteLine(element);
                }
            Console.ReadKey();
        }
        //iterator
        static IEnumerable<int> Fibs(int fibCount) //zwracamy ciąg liczb Fibonacciego
        {
            for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
            {
                yield return prevFib; //iterator yield
                int newFib = prevFib + curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }
        static IEnumerable<string> Foo()
        {
            yield return "Jeden";
            yield return "Dwa";
            yield return "Trzy";
        }
    }
}
