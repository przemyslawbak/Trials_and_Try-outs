using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lista = new List<string>(File.ReadAllLines("1.txt"));
            List<string> blacklisted = new List<string>(File.ReadAllLines("blacklisted.txt"));

            lista.RemoveAll(x => blacklisted.Any(d => x.Contains(d)));
            File.WriteAllLines("output.txt", lista);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
