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
            foreach (var line in File.ReadLines("1.txt"))
            {
                if (counter == 1 && !string.IsNullOrEmpty(line))
                {
                    toWrite = line;
                    counter = 0;
                }
                if (counter == 2 && !string.IsNullOrEmpty(line))
                {
                    toWrite = toWrite + "|" + line;
                    counter = 0;
                }
                if (counter == 3 && !string.IsNullOrEmpty(line))
                {
                    toWrite = toWrite + ", " + line;
                    counter = 0;
                }
                if (line.Contains("Country"))
                {
                    counter = 2;
                }
                if (line.Contains("Address"))
                {
                    counter = 3;
                }
                if (line.Contains("VACANCIES"))
                {
                    toWrite = toWrite + Environment.NewLine;
                    File.AppendAllText("output.txt", toWrite);
                    counter = 1;
                }

            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
