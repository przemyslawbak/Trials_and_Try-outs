using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lista = File.ReadAllLines("1.txt");
            List<string> pierwotnaLista = new List<string>(lista);
            List<string> items = new List<string>();

            foreach (var item in pierwotnaLista)
            {
                if (item.Contains(","))
                {
                    string[] it = item.Split(',');
                    foreach (var iii in it)
                    {
                        items.Add(iii);
                    }
                }
                else
                {
                    items.Add(item);
                }
            }

            List<string> robocza = items.Distinct().OrderBy(r => r).ToList();
            List<string> output = new List<string>();
            foreach (var item in robocza)
            {
                Console.WriteLine(item);

                output.Add(item);


            }
            System.IO.File.WriteAllLines("output.txt", output);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
