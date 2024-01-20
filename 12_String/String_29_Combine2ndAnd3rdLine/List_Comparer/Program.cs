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
                if (counter == 1)
                {
                    toWrite = line + "|";
                    counter++;
                }
                else if (counter == 2)
                {
                    counter++;
                }
                else if (counter == 3)
                {
                    toWrite = toWrite + line + Environment.NewLine;
                    counter = 1;
                    File.AppendAllText("output.txt", toWrite);
                    toWrite = "";
                }
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
