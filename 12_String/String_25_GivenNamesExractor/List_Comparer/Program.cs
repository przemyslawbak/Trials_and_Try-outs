using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        private static List<string> _givenNames = new List<string>();

        static void Main(string[] args)
        {
            foreach (string file in Directory.EnumerateFiles(Path.Combine("data"), "*.*"))
            {
                Console.WriteLine("Reading " + file);
                List<string> contents = File.ReadAllLines(file).ToList();
                List<string> names = contents.Where(c => c.Contains(",")).Select(i => i.Split(',')[0]).ToList();
                _givenNames.AddRange(names);
            }

            File.WriteAllLines("output2.txt", _givenNames.Distinct());
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
