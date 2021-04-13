using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] one = File.ReadAllLines("1.txt"); //do wykluczenia
            string[] two = File.ReadAllLines("2.txt"); //podstawa

            List<string> second = two.ToList();
            List<string> first = one.ToList();

            List<string> missing = second.Except(first).ToList();

            System.IO.File.WriteAllLines("missing.txt", missing);
            Console.WriteLine("saved");
            Console.ReadKey();
        }
    }
}
