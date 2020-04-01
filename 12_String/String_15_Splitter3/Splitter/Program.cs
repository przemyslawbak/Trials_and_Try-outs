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
                string[] miasta = File.ReadAllLines("kor.txt");
            foreach (string line in linki)
            {
                string res = "xxx";
                if (line.Contains(","))
                {
                    string[] rrrr = line.Split(',');

                    for (int i = 0; i < rrrr.Length; i++)
                    {
                        if (rrrr[i].Contains("Building"))
                        {
                            foreach (var a in rrrr)
                            {
                                if (miasta.Contains(a.Trim()))
                                {
                                    alllines.Add(a.Trim());
                                    break;
                                }
                            }
                            break;
                        }
                        if (i > 0 && Regex.IsMatch(rrrr[i].Trim(), @"^\d"))
                        {
                            if (rrrr[i - 1].Contains("-do"))
                            {
                                rrrr[i - 1] = rrrr[i - 1].Replace("-do", "");
                            }
                            alllines.Add(rrrr[i - 1].Trim());

                        }
                    }
                }
            }

            File.WriteAllLines("output.txt", alllines);
            Console.ReadKey();
        }
    }
}
