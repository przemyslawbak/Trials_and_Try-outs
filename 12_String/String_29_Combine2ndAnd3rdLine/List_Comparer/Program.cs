using System;
using System.IO;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = 1;
            string toWrite = "";
            var previous = "";
            foreach (var line in File.ReadLines("1.txt"))
            {
                if (counter == 1)
                {
                    if (line.Contains(","))
                    {
                        toWrite = line.Split(',')[0] + "|" + line.Split(',')[1].Trim() + ", ";
                    }
                    else
                    {
                        toWrite = line + "|";
                    }
                    counter++;
                }
                else if (line == "")
                {
                    toWrite = toWrite + previous + Environment.NewLine;
                    counter = 1;
                    File.AppendAllText("output.txt", toWrite);
                    toWrite = "";
                }
                previous = line;
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
