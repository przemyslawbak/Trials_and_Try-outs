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
            //var lll = pierwotnaLista.Where(x => int.TryParse(x, out int ww)).Select(int.Parse).ToList();

            //List<string> output = pierwotnaLista.Distinct().ToList();
            //var output = lll.Distinct().ToList();
            List<string> grouping = pierwotnaLista.GroupBy(email => email).Select(grp => grp.First()).ToList();
            var result = grouping.Where(i => i.Contains("@")).Select(i => ";www." + i.Split('@')[1] + ";;;;;;;;;;;").Distinct();

            System.IO.File.WriteAllLines("output2.txt", result);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
