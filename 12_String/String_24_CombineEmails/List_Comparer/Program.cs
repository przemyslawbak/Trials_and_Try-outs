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
            foreach (string file in Directory.EnumerateFiles(Path.Combine("files"), "*.*"))
            {
                Console.WriteLine("Reading " + file);
                Console.WriteLine("..." + file);
                List<string> contents = File.ReadAllLines(file).ToList();
                List<EmailModel> items = contents.Where(c => c.Contains("|")).Select(i => new EmailModel() { Address = i.Split('|')[1].ToLower(), Company = i.Split('|')[0].ToLower() }).ToList();
                _listFromFiles.AddRange(items);
            }

            string[] lista = File.ReadAllLines("1.csv");
            List<string> listOfFirms = new List<string>(lista);
            int counter = 0;
            foreach (string firm in listOfFirms)
            {
                counter++;
                Console.WriteLine(counter + " of " + listOfFirms.Count());
                string newfirm = firm.ToLower().Replace(".", "");
                List<string> emails = _listFromFiles.Where(l => l.Company.ToLower() == newfirm.ToLower()).Select(l => l.Address.ToLower()).ToList();
                if (emails.Count() > 0)
                {
                    emails = emails.GroupBy(g => g.Trim().ToLower())
                         .Select(g => g.First())
                         .ToList();
                    foreach (string email in emails)
                    {
                        string res = email.ToLower() + "|" + newfirm.ToLower();
                        _results.Add(res);
                    }
                }
                else
                {
                    _missing.Add(newfirm);
                }
            }

            System.IO.File.WriteAllLines("output2.txt", _results.Distinct());
            System.IO.File.WriteAllLines("missing.txt", _missing.Distinct());
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
