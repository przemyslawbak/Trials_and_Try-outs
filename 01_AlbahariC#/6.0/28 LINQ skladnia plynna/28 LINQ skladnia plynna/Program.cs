using System;
using System.Collections.Generic;
using System.Linq;

namespace _28_LINQ_skladnia_plynna
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };

            //łańcuch
            IEnumerable<string> query = names
            .Where(n => n.Contains("a"))
            .OrderBy(n => n.Length)
            .Select(n => n.ToUpper());
            foreach (string name in query) Console.WriteLine(name);

            //na piechotę
            IEnumerable<string> filtered = names.Where(n => n.Contains("a"));
            IEnumerable<string> sorted = filtered.OrderBy(n => n.Length);
            IEnumerable<string> finalQuery = sorted.Select(n => n.ToUpper());
            foreach (string name in filtered)
                Console.Write(name + "|"); // Jan|Olga|Daria|
            Console.WriteLine();
            foreach (string name in sorted)
                Console.Write(name + "|"); // Jan|Olga|Daria|
            Console.WriteLine();
            foreach (string name in finalQuery)
                Console.Write(name + "|"); // JAN|OLGA|DARIA|

            IEnumerable<int> query1 = names.Select(n => n.Length);
            foreach (int length in query1)
                Console.Write(length + "|"); // 3|4|5|6|5|

            IEnumerable<string> sortedByLength, sortedAlphabetically;
            sortedByLength = names.OrderBy(n => n.Length); // klucz int
            sortedAlphabetically = names.OrderBy(n => n); // klucz string

            //porzadkowanie naturalne
            int[] numbers = { 10, 9, 8, 7, 6 };
            IEnumerable<int> firstThree = numbers.Take(3); // { 10, 9, 8 }
            IEnumerable<int> lastTwo = numbers.Skip(3); // { 7, 6 }
            IEnumerable<int> reversed = numbers.Reverse(); // { 6, 7, 8, 9, 10 }
            int firstNumber = numbers.First(); // 10
            int lastNumber = numbers.Last(); // 6
            int secondNumber = numbers.ElementAt(1); // 9
            int secondLowest = numbers.OrderBy(n => n).Skip(1).First(); // 7
            int count = numbers.Count(); // 5;
            int min = numbers.Min(); // 6;
            bool hasTheNumberNine = numbers.Contains(9); // prawda
            bool hasMoreThanZeroElements = numbers.Any(); // prawda
            bool hasAnOddElement = numbers.Any(n => n % 2 != 0); // prawda
            int[] seq1 = { 1, 2, 3 };
            int[] seq2 = { 3, 4, 5 };
            IEnumerable<int> concat = seq1.Concat(seq2); // { 1, 2, 3, 3, 4, 5 }
            IEnumerable<int> union = seq1.Union(seq2); // { 1, 2, 3, 4, 5 }

            //wykonanie opóźnione
            var numberss = new List<int>();
            numberss.Add(1);
            IEnumerable<int> queryy = numbers.Select(n => n * 10); // budowa zapytania
            numberss.Add(2); // dodanie elementu
            foreach (int n in queryy)
                Console.Write(n + "|"); // 10|20|

            //przechowywanie zmiennych
            int[] numbersz = { 1, 2 };
            int factor = 10;
            IEnumerable<int> queryz = numbers.Select(n => n * factor);
            factor = 20;
            foreach (int n in queryz) Console.Write(n + "|"); // 20|40|

            //tworzenie dekoratorów
            IEnumerable<int> queryq = new int[] { 5, 12, 3 }.Where(n => n < 10)
            .OrderBy(n => n)
            .Select(n => n * 10);

            //stopniowe składanie zapytania
            IEnumerable<int>
            source = new int[] { 5, 12, 3 },
            filteredd = source.Where(n => n < 10),
            sortedd = filteredd.OrderBy(n => n),
            queryd = sortedd.Select(n => n * 10);

            //metoda progresywna
            IEnumerable<string> queryr = names
            .Select(n => n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "").Replace("y", ""))
            .Where(n => n.Length > 2)
            .OrderBy(n => n);


            Console.ReadKey();
        }
    }
}
