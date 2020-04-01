using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                string res = "xxx";
                if (line.Contains(","))
                {
                    List<string> items = line.Split(',').Reverse().ToList();
                    if (items.Count() == 1)
                    {
                        res = "";
                    }
                    else
                    {
                        if (items[1].Any(char.IsDigit))
                        {
                            res = Regex.Replace(items[1], @"[\d-]", string.Empty).Replace("-", "").Trim();
                        }
                    }
                }

                alllines.Add(res);
            }

            File.WriteAllLines("output.txt", alllines);
            Console.ReadKey();
        }
    }
}
