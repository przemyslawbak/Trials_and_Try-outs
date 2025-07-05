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
        private static Hidden _hidden;
        static void Main(string[] args)
        {
            _hidden = new Hidden();
            List<string> adresy = new List<string>();
            List<string> konta = new List<string>(File.ReadAllLines("1.txt"));
            foreach (string konto in konta)
            {
                Console.WriteLine();
                Console.WriteLine("Opening now: " + konto);
                try
                {
                    using (Pop3Client client = new Pop3Client())
                    {
                        client.Connect(_hidden.GetHost(), _hidden.GetPort(), MailKit.Security.SecureSocketOptions.None);
                        client.Authenticate(konto, _hidden.GetPass());
                        var count = client.GetMessageCount();
                        for (int i = 0; i < client.Count; i++)
                        {
                            var wiadomosc = client.GetMessage(i);
                            string temat = wiadomosc.Subject.ToString();
                            string tresc = wiadomosc.Body.ToString();
                            if (temat.Contains("Undeliverable") || (temat.Contains("(Failure)")))
                            {
                                if (tresc.Contains("@"))
                                {
                                    ModelDanych dane = new ModelDanych();
                                    dane.Tytul = temat;
                                    string[] words = tresc.Split(" ");
                                    foreach (var word in words)
                                    {
                                        if (word.Contains("@") && !word.Contains("myworkemail.net"))
                                        {
                                            string replacement = Regex.Replace(word, @"\t|\n|\r", "");
                                            if (replacement.Contains("mailto:"))
                                            {
                                                dane.AdresEmail = replacement.Split('(')[1].Split(')')[0];
                                            }
                                            else
                                            {
                                                dane.AdresEmail = replacement.ToLower().Trim().Replace("<", "").Replace(">", "");
                                            }
                                            Console.WriteLine(dane.AdresEmail);

                                            if (dane.AdresEmail.Contains("@"))
                                            {
                                                adresy.Add(dane.AdresEmail);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        client.Disconnect(true);
                        Console.WriteLine("Opening next...");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + " in: " + konto);
                    Console.WriteLine("Press any key...");
					Console.ReadKey();
                }
            }

            File.AppendAllLines("Savedlist.txt", adresy);
            Console.WriteLine("Koniec");
            Console.WriteLine("Press any key...");
			Console.ReadKey();

        }
    }
}
