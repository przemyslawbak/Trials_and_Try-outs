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

            //List<string> output = pierwotnaLista.Distinct().ToList();
            var result = pierwotnaLista.GroupBy(test => test)
                   .Select(grp => grp.First())
                   .ToList();

            System.IO.File.WriteAllLines("output2.txt", result);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
