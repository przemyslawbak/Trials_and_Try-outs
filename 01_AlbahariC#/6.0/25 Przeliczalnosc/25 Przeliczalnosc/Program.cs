using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace _25_Przeliczalnosc// ICollection i IList i IReadOnlyList i Array
{
    public interface ICollection<T> : IEnumerable<T>, IEnumerable
    {
        int Count { get; }
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        bool IsReadOnly { get; }
        void Add(T item);
        bool Remove(T item);
        void Clear();
    }
    public interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[int index] { get; set; }
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);
    }
    public interface IList : ICollection, IEnumerable
    {
        object this[int index] { get; set }
        bool IsFixedSize { get; }
        bool IsReadOnly { get; }
        int Add(object value);
        void Clear();
        bool Contains(object value);
        int IndexOf(object value);
        void Insert(int index, object value);
        void Remove(object value);
        void RemoveAt(int index);
    }
    class Program
    {
        public static IEnumerable<int> GetSomeIntegers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
        static void Main(string[] args)
        {
            foreach (int i in Program.GetSomeIntegers())
                Console.WriteLine(i);
            Console.ReadKey();

            StringBuilder[] builders = new StringBuilder[5];
            builders[0] = new StringBuilder("builder1");
            builders[1] = new StringBuilder("builder2");
            builders[2] = new StringBuilder("builder3");
            long[] numbers = new long[3];
            numbers[0] = 12345;
            numbers[1] = 54321;

            object[] a1 = { "string", 123, true };
            object[] a2 = { "string", 123, true };
            Console.WriteLine(a1 == a2); // fałsz
            Console.WriteLine(a1.Equals(a2)); // fałsz
            IStructuralEquatable se1 = a1;
            Console.WriteLine(se1.Equals(a2,
            StructuralComparisons.StructuralEqualityComparer)); // prawda
            //clone
            StringBuilder[] builders2 = builders;
            StringBuilder[] shallowClone = (StringBuilder[])builders.Clone();
            int[] myArray = { 1, 2, 3 };
            int first = myArray[0];
            int last = myArray[myArray.Length - 1];
            // utworzenie tablicy łańcuchów o długości 2
            Array a = Array.CreateInstance(typeof(string), 2);
            a.SetValue("Witaj,", 0); // → a[0] = "Witaj,";
            a.SetValue("świecie", 1); // → a[1] = "świecie";
            string s = (string)a.GetValue(0); // → s = a[0];
                                              // istnieje też możliwość rzutowania na tablice C#
            string[] cSharpArray = (string[])a;
            string s2 = cSharpArray[0];
            int[] oneD = { 1, 2, 3 };
            int[,] twoD = { { 5, 6 }, { 8, 9 } };
            string[] names = { "Robert", "Jacek", "Juliusz" };
            string match = Array.Find(names, ContainsA);
            Console.WriteLine(match); // Jacek

            Console.ReadKey();
        }
        static bool ContainsA(string name) { return name.Contains("a"); }
    }
}
