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
            List<string> blacklisted = new List<string>(File.ReadAllLines("2.txt"));
            List<string> blacklist = new List<string>(File.ReadAllLines("3.txt"));
            List<string> lista = new List<string>(File.ReadAllLines("1.txt"));
            List<string> containBlacklisted = lista.Where(e => blacklist.Any(k => e.Contains(k.ToLower()))).ToList();
            lista = lista.Except(containBlacklisted).ToList();

            lista.RemoveAll(x => blacklisted.Any(d => x.ToLower().Contains(d.ToLower())));
            File.WriteAllLines("output.txt", lista);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
