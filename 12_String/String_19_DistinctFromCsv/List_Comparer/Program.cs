using CsvHelper;
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
            List<string> failedEmails = new List<string>(File.ReadAllLines("failed.txt"));
            string[] keywords = new string[] { "info", "office", "conta", "admission", "agen", "career", "appl", "crew", "email", "enquir", "general", "hr@", "cv@", "jobs", "mail@", "recruit", "general", "marine", "offshore", "personnel", "resume" };
            string[] blacklist = new string[] { "ir@", "admin", "corporate", "spam", "account", "invoice", "charter", "book", "business", "cruise", "customer", "protection", "invest", "financ", "firstname", "lastname", "freight", "hotel", "airport", "it@", "library", "marketing", "media", "privacy", "proxy", "purchasing", "reservations", "sales", "webmaster"};
            List<string> items = new List<string>();
            CsvReader csv = new CsvReader(File.OpenText("all.txt"), System.Globalization.CultureInfo.CurrentCulture);
            List<EmailModel> myCustomObjects = csv.GetRecords<EmailModel>().ToList();

            var distEmail = myCustomObjects
                .GroupBy(e => e.Email)
                .Select(e => e.First())
                .ToList();

            List<EmailModel> containBlacklisted = distEmail.Where(e => blacklist.Any(k => e.Email.ToLower().Contains(k.ToLower()))).ToList();
            List<EmailModel> containFailed = distEmail.Where(e => failedEmails.Any(k => e.Email.ToLower().Contains(k.ToLower()))).ToList();

            distEmail = distEmail.Except(containBlacklisted).ToList();
            distEmail = distEmail.Except(containFailed).ToList();

            List<EmailModel> containKeys = distEmail.Where(e => keywords.Any(k => e.Email.ToLower().Contains(k.ToLower()))).ToList();

            List<EmailModel> notContainKeysFirst = distEmail.Except(containKeys).GroupBy(c => c.Company).Select(c => c.ElementAt(0)).ToList();
            List<EmailModel> notContainKeysSecond = distEmail.Except(containKeys).GroupBy(c => c.Company).Where(x => x.Count() > 1).Select(c => c.ElementAt(1)).ToList();
            List<EmailModel> notContainKeysThird = distEmail.Except(containKeys).GroupBy(c => c.Company).Where(x => x.Count() > 2).Select(c => c.ElementAt(2)).ToList();

            List<string> final = new List<string>();
            final.AddRange(containKeys.Select(l => l.Email));
            final.AddRange(notContainKeysFirst.Select(l => l.Email));
            final.AddRange(notContainKeysSecond.Select(l => l.Email));
            final.AddRange(notContainKeysThird.Select(l => l.Email));

            
            System.IO.File.WriteAllLines("output.txt", final);
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
