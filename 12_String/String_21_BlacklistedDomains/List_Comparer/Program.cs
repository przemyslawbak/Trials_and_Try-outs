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
            List<string> toRemove = new List<string>(File.ReadAllLines("2.txt"));
            List<string> blacklist = new List<string>(File.ReadAllLines("3.txt"));
            List<string> phrasesBanned = new List<string>(File.ReadAllLines("4.txt"));
            List<string> lista = new List<string>(File.ReadAllLines("1.txt"));
            List<string> containBlacklisted = lista.Where(e => blacklist.Any(k => e.ToLower().Contains(k.ToLower()))).ToList();
            List<string> containPhrases = lista.Where(e => phrasesBanned.Any(k => e.ToLower().Contains(k.ToLower()))).ToList();
            List<string> removed = lista.Where(e => toRemove.Any(k => e.ToLower().Contains(k.ToLower()))).ToList();
            lista = lista.Except(containBlacklisted).ToList();
            lista = lista.Except(containPhrases).ToList();
            lista = lista.Except(removed).ToList();

            //lista.RemoveAll(x => toRemove.Any(d => x.ToLower().Contains(d.ToLower())));
            File.WriteAllLines("output.txt", lista);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
