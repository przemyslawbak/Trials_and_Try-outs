using System;
using System.Collections.Generic;
using System.IO;

namespace Splitter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> alllines = new List<string>();
                string[] linki = File.ReadAllLines("1.txt");
            foreach (string line in linki)
            {
                string res = line.Trim();

                alllines.Add(res.Trim());
            }

            File.WriteAllLines("output.txt", alllines);
            Console.ReadKey();
        }
    }
}
