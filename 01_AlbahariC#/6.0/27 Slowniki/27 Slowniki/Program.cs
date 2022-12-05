using System;
using System.Collections.Generic;
using System.Reflection;

namespace _27_Slowniki
{
    class Program
    {
        static bool Foo<T>(T x, T y)
        {
            bool same = EqualityComparer<T>.Default.Equals(x, y);
            return same;
        }
            static void Main(string[] args)
        {
            //slowniki niesortowane
            var d = new Dictionary<string, int>();
            d.Add("Jeden", 1);
            d["Dwa"] = 2; // dodaje element do słownika, ponieważ nie ma w nim jeszcze "dwa"
            d["Dwa"] = 22; // modyfikuje słownik, ponieważ "dwa" już w nim jest
            d["Trzy"] = 3;
            Console.WriteLine(d["Dwa"]); // drukuje "22"
            Console.WriteLine(d.ContainsKey("Jeden")); // prawda (szybka operacja)
            Console.WriteLine(d.ContainsValue(3)); // prawda (wolna operacja)
            int val = 0;
            if (!d.TryGetValue("jedeN", out val))
                Console.WriteLine("Brak"); // "Brak" (wielkość liter ma znaczenie)
                                           // trzy różne sposoby przeglądania słownika
            foreach (KeyValuePair<string, int> kv in d) // Jeden; 1
                Console.WriteLine(kv.Key + "; " + kv.Value); // Dwa; 22
                                                             // Trzy; 3
            foreach (string s in d.Keys) Console.Write(s); // JedenDwaTrzy
            Console.WriteLine();
            foreach (int i in d.Values) Console.Write(i); // 1223
            Console.WriteLine("slowniki sortowane:");

            //slowniki sortowane
            var sorted = new SortedList<string, MethodInfo>();
            foreach (MethodInfo m in typeof(object).GetMethods())
                sorted[m.Name] = m;
            foreach (string name in sorted.Keys)
                Console.WriteLine(name);
            foreach (MethodInfo m in sorted.Values)
                Console.WriteLine(m.Name + " zwraca obiekt typu " + m.ReturnType);

            //porownanie bez przeslonienia
            Customer c1 = new Customer("Barański", "Jan");
            Customer c2 = new Customer("Barański", "Jan");
            Console.WriteLine(c1 == c2); // fałsz
            Console.WriteLine(c1.Equals(c2)); // fałsz
            var e = new Dictionary<Customer, string>();
            e[c1] = "Jan";
            Console.WriteLine(e.ContainsKey(c2)); // fałsz
            //wlasny comparer
            var eqComparer = new LastFirstEqComparer();
            var f = new Dictionary<Customer, string>(eqComparer);
            f[c1] = "Jan";
            Console.WriteLine(f.ContainsKey(c2)); // prawda

            //metoda statyczna
            Console.WriteLine("metoda: " + Foo(c1.FirstName,c2.FirstName));

            //IComparer sortowanie
            var wishList = new List<Wish>();
            wishList.Add(new Wish("Pokój", 2));
            wishList.Add(new Wish("Bogactwo", 3));
            wishList.Add(new Wish("Miłość", 2));
            wishList.Add(new Wish("3 kolejne życzenia", 1));
            wishList.Sort(new PriorityComparer());
            foreach (Wish w in wishList) Console.Write(w.Name + " | ");
            Console.ReadKey();
        }
    }
    public class Customer
    {
        public string LastName;
        public string FirstName;
        public Customer(string last, string first)
        {
            LastName = last;
            FirstName = first;
        }
    }
    //porownywanie niegeneryczna klasa
    public class LastFirstEqComparer : EqualityComparer<Customer>
    {
        public override bool Equals(Customer x, Customer y)
        => x.LastName == y.LastName && x.FirstName == y.FirstName;
        public override int GetHashCode(Customer obj)
        => (obj.LastName + ";" + obj.FirstName).GetHashCode();
    }

    //IComparer
    class Wish
    {
        public string Name;
        public int Priority;
        public Wish(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }
    }
    class PriorityComparer : Comparer<Wish>
    {
        public override int Compare(Wish x, Wish y)
        {
            if (object.Equals(x, y)) return 0; // test bezpieczeństwa
            return x.Priority.CompareTo(y.Priority);
        }
    }
}
