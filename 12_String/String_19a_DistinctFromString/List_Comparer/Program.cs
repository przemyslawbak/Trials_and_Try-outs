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
            string[] blacklist = new string[] { "ir@", "admin", "corporate", "spam", "account", "invoice", "charter", "book", "business", "cruise", "customer", "protection", "invest", "financ", "firstname", "lastname", "freight", "hotel", "airport", "it@", "library", "marketing", "media", "privacy", "proxy", "purchasing", "reservation", "sale", "webmaster", "customer", "abuse" };
            List<string> items = new List<string>();
            string[] lista = File.ReadAllLines("1.txt");
            List<string> emails = new List<string>(lista).Distinct().ToList();

            List<string> containBlacklisted = emails.Where(e => blacklist.Any(k => e.ToLower().Contains(k.ToLower()))).ToList();
            List<string> containFailed = emails.Where(e => failedEmails.Any(k => e.ToLower().Contains(k.ToLower()))).ToList();
            emails = emails.Except(containBlacklisted).ToList();
            emails = emails.Except(containFailed).ToList();

            List<string> domains = emails.Where(e => e.Contains("@")).Select(e => e.Split('@')[1]).ToList();




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
