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

            File.WriteAllLines("_output.txt", newFileRows);

            Console.WriteLine("Saved...");
            Console.ReadLine();
        }

        private static string ConvertRow(string row)
        {
            row = row.Replace("\t", "");
            row = row.Replace("DataOpen(5)", "data[i - 5].Open");
            row = row.Replace("DataOpen(4)", "data[i - 4].Open");
            row = row.Replace("DataOpen(3)", "data[i - 3].Open");
            row = row.Replace("DataOpen(2)", "data[i - 2].Open");
            row = row.Replace("DataOpen(1)", "data[i - 1].Open");
            row = row.Replace("DataOpen(0)", "data[i - 0].Open");

            row = row.Replace("DataHigh(5)", "data[i - 5].High");
            row = row.Replace("DataHigh(4)", "data[i - 4].High");
            row = row.Replace("DataHigh(3)", "data[i - 3].High");
            row = row.Replace("DataHigh(2)", "data[i - 2].High");
            row = row.Replace("DataHigh(1)", "data[i - 1].High");
            row = row.Replace("DataHigh(0)", "data[i - 0].High");

            row = row.Replace("DataLow(5)", "data[i - 5].Low");
            row = row.Replace("DataLow(4)", "data[i - 4].Low");
            row = row.Replace("DataLow(3)", "data[i - 3].Low");
            row = row.Replace("DataLow(2)", "data[i - 2].Low");
            row = row.Replace("DataLow(1)", "data[i - 1].Low");
            row = row.Replace("DataLow(0)", "data[i - 0].Low");

            row = row.Replace("DataClose(5)", "data[i - 5].Close");
            row = row.Replace("DataClose(4)", "data[i - 4].Close");
            row = row.Replace("DataClose(3)", "data[i - 3].Close");
            row = row.Replace("DataClose(2)", "data[i - 2].Close");
            row = row.Replace("DataClose(1)", "data[i - 1].Close");
            row = row.Replace("DataClose(0)", "data[i - 0].Close");

            row = row.Replace("return 5;", " data[i].Signal = true;");
            row = row.Replace("return 4;", " data[i].Signal = true;");
            row = row.Replace("return 3;", " data[i].Signal = true;");
            row = row.Replace("return 2;", " data[i].Signal = true;");
            row = row.Replace("return 1;", " data[i].Signal = true;");
            row = row.Replace("return 0;", " data[i].Signal = true;");

            if (row.StartsWith("//"))
            {
                return row;
            }
            else if (row.StartsWith("-"))
            {
                row = row.Replace(" ", "");
                row = row.Replace("-", "} return data; } public List<OhlcvObject> ");
                row = row + "(List<OhlcvObject> data) { for (int i = 5; i < data.Count; i++) {";
            }

            return row;
        }
    }
}
