using System;
using System.Data.SqlTypes;
using System.IO;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = 1;
            string toWrite = "";
            string last = "";
            foreach (var line in File.ReadLines("1.txt"))
            {
                if (counter == 1 && !string.IsNullOrEmpty(line) && last != line)
                {
                    toWrite = line;
                    counter++;
                }
                else if (counter == 2 && line.Contains("views"))
                {
                    toWrite = toWrite + "|" + line.Split(',')[1].Trim();
                    counter = 0;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    toWrite = toWrite + Environment.NewLine;
                    File.AppendAllText("output.txt", toWrite);
                    counter = 1;
                    toWrite = "";
                }

                last = line;
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
