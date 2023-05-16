using System;
using System.Collections.Generic;
using System.IO;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] file = File.ReadAllLines("1.txt");

            List<string> newFileRows = new List<string>();

            foreach (var row in file)
            {
                newFileRows.Add(ConvertRow(row));
            }

            Console.ReadLine();
        }

        private static string ConvertRow(string row)
        {
            row = row.Replace("\t", "");

            if (row.StartsWith("//"))
            {
                return row;
            }
            else if (row.StartsWith("-"))
            {
                row = row.Replace(" ", "");
                row = row.Replace("-", "private List<OhlcvObject> ");
                row = row + "(List<OhlcvObject> data)";
            }

            return row;
        }
    }
}
