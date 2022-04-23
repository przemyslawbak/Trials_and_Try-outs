using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _30_LINQ_operatory
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "Tomek", "Darek", "Henryk", "Maria", "Janusz" };

            //filtrowanie
            //endswith
            IEnumerable<string> query = from n in names
                                        where n.EndsWith("k")
                                        select n;
            foreach (string name in query) Console.WriteLine(name);
            //where
            query = names.Where((n, i) => i % 2 == 0);
            // wynik { "Tomek", "Henryk", "Janusz" }
            foreach (string name in query) Console.WriteLine(name);
            //Metody TakeWhile i SkipWhile
            int[] numbers = { 3, 5, 2, 234, 4, 1 };
            var takeWhileSmall = numbers.TakeWhile(n => n < 100); // { 3, 5, 2 }
            foreach (int number in takeWhileSmall) Console.WriteLine(number);
            var skipWhileSmall = numbers.SkipWhile(n => n < 100); // { 234, 4, 1 }
            foreach (int number in skipWhileSmall) Console.WriteLine(number);
            //distinct
            char[] distinctLetters = "Witajświecie".Distinct().ToArray();
            string s = new string(distinctLetters); // Witajśec
            //Projekcja z indeksowaniem
            string[] namess = { "Tomek", "Darek", "Henryk", "Maria", "Janusz" };
            IEnumerable<string> queryy = namess
            .Select((ss, i) => i + "=" + ss); // { "0=Tomek", "1=Darek", ... }
            //select many
            string[] fullNames = { "Robert Siwiak", "Mirosława Michalina Włodarczyk", "Mariusz Romańczuk" };
            string testInputElement = "Robert Siwiak";
            string[] childSequence = testInputElement.Split();
            IEnumerable<string> querey = fullNames.SelectMany(name => name.Split());
            foreach (string name in querey)
                Console.Write(name + "|"); // Robert|Siwiak|Mirosława|Michalina|Włodarczyk|Mariusz|Romańczuk|
            //zmienne zakresowe
            IEnumerable<string> querry =
            from fullName in fullNames
            from name in fullName.Split()
            select name + " pochodzi z " + fullName;
            /*Robert pochodzi z Robert Siwiak
            Mariusz pochodzi z Mariusz Romańczuk
            Mirosława pochodzi z Mirosława Michalina Włodarczyk*/
            IEnumerable<string> querwy = fullNames
            .SelectMany(fName => fName.Split()
            .Select(name => new { name, fName }))
            .OrderBy(x => x.fName)
            .ThenBy(x => x.name)
            .Select(x => x.name + " pochodzi z " + x.fName);
            foreach(var item in querwy)
                Console.WriteLine(item);
            //łączenie za pomocą SelectMany
            string[] playerss = { "Tomek", "Janusz", "Maria" };
            IEnumerable<string> querty = from name1 in playerss
                                        from name2 in playerss
                                        select name1 + " vs " + name2;
            //zip
            int[] nuqmbers = { 3, 5, 7 };
            string[] words = { "trzy", "pięć", "siedem", "zignorowany" };
            IEnumerable<string> zip = nuqmbers.Zip(words, (n, w) => n + "=" + w);
            //porządkowanie
            IEnumerable<string> quersy = names.OrderBy(x => x.Length);
            query = names.OrderBy(t => t.Length).ThenBy(t => t);
            IOrderedEnumerable<string> query1 = names.OrderBy(u => u.Length);
            IOrderedEnumerable<string> query2 = query1.ThenBy(w => w);
            //zbiory
            int[] seq1 = { 1, 2, 3 }, seq2 = { 3, 4, 5 };
            IEnumerable<int>
            concat = seq1.Concat(seq2), // { 1, 2, 3, 3, 4, 5 }
            union = seq1.Union(seq2); // { 1, 2, 3, 4, 5 }
            MethodInfo[] methods = typeof(string).GetMethods();
            PropertyInfo[] props = typeof(string).GetProperties();
            IEnumerable<MemberInfo> both = methods.Concat<MemberInfo>(props);
            IEnumerable<int> commonality = seq1.Intersect(seq2), // { 3 }
            difference1 = seq1.Except(seq2), // { 1, 2 }
            difference2 = seq2.Except(seq1); // { 4, 5 }
            //Metody konwersji
            ArrayList classicList = new ArrayList(); // z System.Collections
            classicList.AddRange(new int[] { 3, 4, 5 });
            IEnumerable<int> sequence1 = classicList.Cast<int>();
            DateTime offender = DateTime.Now;
            classicList.Add(offender);
            IEnumerable<int>
            sequence2 = classicList.OfType<int>(), // OK — ignoruje element typu DateTime
            sequence3 = classicList.Cast<int>(); // zgłasza wyjątek
            //Operatory elementów
            numbers = new int[] { 1, 2, 3, 4, 5 };
            int first = numbers.First();
            int last = numbers.Last();
            int firstEven = numbers.First(n => n % 2 == 0);
            int lastEven = numbers.Last(n => n % 2 == 0);
            int firstBigError = numbers.First(n => n < 10);
            int firstBigNumber = numbers.FirstOrDefault(n => n > 10);
            int onlyDivBy3 = numbers.Single(n => n % 3 == 0);
            int noMatches = numbers.SingleOrDefault(n => n > 10);
            int third = numbers.ElementAt(2); // 3
            //int tenthError = numbers.ElementAt(9); // wyjątek
            int tenth = numbers.ElementAtOrDefault(9); // 0
            //Metody agregacyjne
            int fullCount = new int[] { 5, 6, 7 }.Count(); // 3
            int digitCount = "pa55w0rd".Count(c => char.IsDigit(c)); // 3
            numbers = new int[] { 28, 32, 14 };
            int smallest = numbers.Min(); // 14;
            int largest = numbers.Max(); // 32;
            int smallestt = numbers.Max(n => n % 10); // 8;
            //Kwantyfikatory
            bool hasAThree = new int[] { 2, 3, 4 }.Contains(3); // true;
            bool hasAThreee = new int[] { 2, 3, 4 }.Any(n => n == 3); // true;
            bool hasABigNumber = new int[] { 2, 3, 4 }.Any(n => n > 10); // false;
            bool hasABigNumberr = new int[] { 2, 3, 4 }.Where(n => n > 10).Any();
            //Metody generujące
            foreach (string a in Enumerable.Empty<string>())
                Console.Write(a); // <nic>
            int[][] numbersq =
                {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                null // ta wartość null sprawi, że poniższe zapytanie nie zostanie wykonane
                };
            IEnumerable<int> flat = numbersq.SelectMany(innerArray => innerArray);
            flat = numbersq
                .SelectMany(innerArray => innerArray ?? Enumerable.Empty<int>());
            foreach (int i in flat)
                Console.Write(i + " "); // 1 2 3 4 5 6
            foreach (int i in Enumerable.Range(5, 3))
                Console.Write(i + " "); // 5 6 7
            foreach (bool x in Enumerable.Repeat(true, 3))
                Console.Write(x + " "); // True True True
            Console.ReadKey();
        }
    }
}
