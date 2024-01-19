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
                    toWrite = line;
                }
                if (counter == 2) 
                {
                    toWrite = toWrite + "|" + line;
                }
                if (counter == 3) 
                {
                    toWrite = toWrite + ", " + line + Environment.NewLine;
                }

                if (counter == 3)
                {
                    File.AppendAllText("output.txt", toWrite);
                    counter = 1;
                }
                else
                {
                    counter++;
                }
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
