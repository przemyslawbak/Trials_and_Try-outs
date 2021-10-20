using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        private static List<EmailModel> _listFromFiles = new List<EmailModel>();

        static void Main(string[] args)
        {
            Console.WriteLine("Reading all files...");
            foreach (string file in Directory.EnumerateFiles(Path.Combine("files"), "*.csv"))
            {
                Console.WriteLine("Reading " + file);
                Console.WriteLine("..." + file);
                List<string> contents = File.ReadAllLines(file).ToList();
                List<EmailModel> items = contents.Where(i => !i.Contains("\0")).Select(i => new EmailModel() { Address = i.Split('|')[0], Company = i.Split('|')[1] }).ToList();
                _listFromFiles.AddRange(items);
            }

            string[] lista = File.ReadAllLines("1.txt");
            List<string> listOfFirms = new List<string>(lista);




            //var lll = pierwotnaLista.Where(x => int.TryParse(x, out int ww)).Select(int.Parse).ToList();

            //List<string> output = pierwotnaLista.Distinct().ToList();
            //var output = lll.Distinct().ToList();
            //List<string> grouping = pierwotnaLista.GroupBy(email => email).Select(grp => grp.First()).ToList();
            //var result = grouping.Where(i => i.Contains("@")).Select(i => ";www." + i.Split('@')[1] + ";;;;;;;;;;;").Distinct();

            //System.IO.File.WriteAllLines("output2.txt", result);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
