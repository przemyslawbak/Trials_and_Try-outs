using MailKit.Net.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EmailCrawler_01_Trial
{
    public class ModelDanych
    {
        public int ID { get; set; }
        public string Tytul { get; set; }
        public string AdresEmail { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> adresy = new List<string>();
            List<string> konta = new List<string>(File.ReadAllLines("1.txt"));
            foreach (string konto in konta)
            {
                try
                {
                    using (Pop3Client client = new Pop3Client())
                    {
                        client.Connect("pop.email-you.com", 110, MailKit.Security.SecureSocketOptions.None);
                        client.Authenticate(konto, "xxx");
                        var count = client.GetMessageCount();
                        for (int i = 0; i < client.Count; i++)
                        {
                            var wiadomosc = client.GetMessage(i);
                            string temat = wiadomosc.Subject.ToString();
                            string tresc = wiadomosc.Body.ToString();
                            if (temat.Contains("Mail delivery failed"))
                            {
                                if (tresc.Contains("@"))
                                {
                                    ModelDanych dane = new ModelDanych();
                                    dane.Tytul = temat;
                                    string[] words = tresc.Split(" ");
                                    foreach (var word in words)
                                    {
                                        if (word.Contains("@"))
                                        {
                                            string replacement = Regex.Replace(word, @"\t|\n|\r", "");
                                            dane.AdresEmail = replacement;
                                            Console.WriteLine(dane.AdresEmail);
                                            adresy.Add(dane.AdresEmail);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        client.Disconnect(true);
                        Console.WriteLine("opening next...");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            File.AppendAllLines("savedlist.txt", adresy);
            Console.WriteLine("koniec");


        }
    }
}
