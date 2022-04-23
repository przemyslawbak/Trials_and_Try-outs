using System;
using System.Collections.Generic;
using System.Linq;

namespace _29_LINQ_wyrazenia_zapytan
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };

            IEnumerable<string> query =
            from n in names
            where n.Contains("a") // filtrowanie elementów
            orderby n.Length // sortowanie elementów
            select n.ToUpper(); // przekształcanie elementów (projekcja)
            foreach (string name in query) Console.WriteLine(name);

            int matches = (from n in names where n.Contains("a") select n).Count();
            // 4
            string first = (from n in names orderby n select n).First(); // Dariusz
            Console.WriteLine(first);
            Console.WriteLine(matches);

            //let
            IEnumerable<string> queryu =
            from n in names
            let vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "").Replace("y", "")
            where vowelless.Length > 2
            orderby vowelless
            select n; // dzięki słowu kluczowemu let n nadal jest w zakresie

            //into
            IEnumerable<string> quersy =
            from n in names
            select n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "").Replace("y", "")
            into noVowel
            where noVowel.Length > 2
            orderby noVowel
            select noVowel;
            Console.ReadKey();
        }
    }
}
