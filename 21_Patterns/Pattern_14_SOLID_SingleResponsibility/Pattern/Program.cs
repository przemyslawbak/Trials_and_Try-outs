using System;
using System.Collections.Generic;
using System.IO;

namespace Pattern
{
    // Zasada pojedynczej odpowiedzialności
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Journal
    {
        private readonly List<string> entries = new List<string>();
        // prosty licznik całkowitej liczby wpisów
        private static int count = 0;

        public void AddEntry(string text) //journal
        {
            entries.Add($"{++count}: {text}");
        }

        public void RemoveEntry(int index) //journal
        {
            entries.RemoveAt(index);
        }
    }

    public class PersistenceManager
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false) //file
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, journal.ToString());
        }
    }
}
