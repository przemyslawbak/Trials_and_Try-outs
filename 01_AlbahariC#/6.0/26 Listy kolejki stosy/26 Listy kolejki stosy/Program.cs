using System;
using System.Collections;
using System.Collections.Generic;

namespace _26_Listy_kolejki_stosy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> words = new List<string>(); // nowa lista typu string
            words.Add("melon");
            words.Add("awokado");
            words.AddRange(new[] { "banan", "pomarańcza" });
            words.Insert(0, "liczi"); // wstawianie na początku
            words.InsertRange(0, new[] { "pomelo", "nashi" }); // wstawianie na początku
            words.Remove("melon");
            words.RemoveAt(3); // usunięcie czwartego elementu
            words.RemoveRange(0, 2); // usunięcie dwóch pierwszych elementów
                                     // usunięcie wszystkich łańcuchów zaczynających się literą 'n'
            words.RemoveAll(s => s.StartsWith("n"));
            Console.WriteLine(words[0]); // pierwsze słowo
            Console.WriteLine(words[words.Count - 1]); // ostatnie słowo
            foreach (string s in words) Console.WriteLine(s); // wszystkie słowa
            List<string> subset = words.GetRange(1, 2); // drugie i trzecie słowo
            string[] wordsArray = words.ToArray(); // tworzy nową tablicę typizowaną
                                                   // kopiowanie pierwszych dwóch elementów na koniec istniejącej tablicy
            string[] existing = new string[1000];
            words.CopyTo(0, existing, 998, 2);
            List<string> upperCastWords = words.ConvertAll(s => s.ToUpper());
            List<int> lengths = words.ConvertAll(s => s.Length);
            ArrayList al = new ArrayList();
            al.Add("hello");
            string first = (string)al[0];
            string[] strArr = (string[])al.ToArray(typeof(string));
            var tune = new LinkedList<string>();
            tune.AddFirst("do"); // do
            tune.AddLast("sol"); // do - sol
            tune.AddAfter(tune.First, "re"); // do - re - sol
            tune.AddAfter(tune.First.Next, "mi"); // do - re - mi - sol
            tune.AddBefore(tune.Last, "fa"); // do - re - mi - fa - sol
            tune.RemoveFirst(); // re - mi - fa - sol
            tune.RemoveLast(); // re - mi - fa
            LinkedListNode<string> miNode = tune.Find("mi");
            tune.Remove(miNode); // re - fa
            tune.AddFirst(miNode); // mi - re - fa
            foreach (string s in tune) Console.WriteLine(s);

            var q = new Queue<int>();
            q.Enqueue(10);
            q.Enqueue(20);
            int[] data = q.ToArray(); // eksport zawartości do tablicy
            Console.WriteLine(q.Count); // "2"
            Console.WriteLine(q.Peek()); // "10"
            Console.WriteLine(q.Dequeue()); // "10"
            Console.WriteLine(q.Dequeue()); // "20"
            //Console.WriteLine(q.Dequeue()); // spowoduje zgłoszenie wyjątku (pusta kolejka)

            var s = new Stack<int>();
            s.Push(1); // s = 1
            s.Push(2); // s = 1,2
            s.Push(3); // s = 1,2,3
            Console.WriteLine(s.Count); // drukuje 3
            Console.WriteLine(s.Peek()); // drukuje 3, s = 1,2,3
            Console.WriteLine(s.Pop()); // drukuje 3, s = 1,2
            Console.WriteLine(s.Pop()); // drukuje 2, s = 1
            Console.WriteLine(s.Pop()); // drukuje 1, s = <empty>
            //Console.WriteLine(s.Pop()); // powoduje wyjątek

            var bits = new BitArray(2);
            bits[1] = true;
            bits.Xor(bits); // bitowe lub wykluczające na samym sobie
            Console.WriteLine(bits[1]); // fałsz

            var letters = new HashSet<char>("gdyby kózka nie skakała");
            Console.WriteLine(letters.Contains('g')); // prawda
            Console.WriteLine(letters.Contains('j')); // fałsz
            foreach (char c in letters) Console.Write(c); // gdyb kózaniesł

            var letterss = new HashSet<char>("gdyby kózka nie skakała");
            letterss.IntersectWith("aeioóuy");
            foreach (char c in letterss) Console.Write(c); // yóaie
            var lettersss = new HashSet<char>("gdyby kózka nie skakała");
            lettersss.ExceptWith("aeioóu");
            foreach (char c in lettersss) Console.Write(c); // gdb kznsł

            var letterssss = new HashSet<char>("gdyby kózka nie skakała");
            letterssss.SymmetricExceptWith("toby nogi nie złamała");
            foreach (char c in letterssss) Console.Write(c); // dkóstom

            Console.ReadKey();
        }
    }
}
