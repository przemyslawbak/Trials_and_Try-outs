using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        private static List<EmailModel> _listFromFiles = new List<EmailModel>();
        private static List<string> _results = new List<string>();
        private static List<string> _missing = new List<string>();

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

            string[] lista = File.ReadAllLines("1.csv");
            List<string> listOfFirms = new List<string>(lista);

            foreach (string firm in listOfFirms)
            {
                List<string> emails = _listFromFiles.Where(l => l.Company.ToLower() == firm.ToLower()).Select(l => l.Address.ToLower()).ToList();
                if (emails != null)
                {
                    foreach (string email in emails)
                    {
                        string res = email.ToLower() + "|" + firm.ToLower();
                        Console.WriteLine(res);
                        _results.Add(res);
                    }
                }
                else
                {
                    _missing.Add(firm);
                }
            }

            System.IO.File.WriteAllLines("output2.txt", _results.Distinct());
            System.IO.File.WriteAllLines("missing.txt", _missing.Distinct());
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
