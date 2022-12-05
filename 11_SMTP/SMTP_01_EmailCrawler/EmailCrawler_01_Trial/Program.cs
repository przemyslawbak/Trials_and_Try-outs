using MailKit.Net.Pop3;
using System;
using System.Collections.Generic;

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
            using (Pop3Client client = new Pop3Client())
            {
                List<ModelDanych> ListaAdresow = new List<ModelDanych>();
                client.Connect("pop.pro-emailing.com", 110, MailKit.Security.SecureSocketOptions.None);
                client.Authenticate("gerald.argarin@pro-emailing.com", "xDt6!ksY6");
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
                                    dane.AdresEmail = word;
                                    Console.Write(dane.AdresEmail);
                                    break;
                                }
                            }
                            ListaAdresow.Add(dane);
                        }
                    }
                }

                client.Disconnect(true);
                Console.WriteLine("koniec");
                Console.ReadKey();
            }
        }
    }
}
