using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("csv.txt"))
            {
                File.Delete("csv.txt");
            }
                string[] firmy = File.ReadAllLines("firmy.txt");
            List<string> nazwyFirm = new List<string>(firmy);
            foreach (string nazwa in nazwyFirm)
            {
                string calosc = "";
                string srodek = "";
                string[] tokens = nazwa.Split(',');
                foreach (string token in tokens)
                {
                    token.Trim(' ');
                }
                string poczatek = tokens[0] + "|" + tokens[1] + "|" + tokens[2] + "|" + tokens[3];
                if (tokens[4].StartsWith("Care of"))
                {
                    srodek = poczatek + "|" + tokens[4] + "|";
                    tokens[4] = "";
                }
                else
                {
                    srodek = poczatek + "|NONE|";
                }
                calosc = calosc + srodek;
                for (int i = 4; i < tokens.Length-1; i++)
                {
                    if (tokens[i] != "")
                    {
                        if (i != (tokens.Length - 2))
                        {
                            calosc = calosc + tokens[i] + ",";
                        }
                        else
                        {
                            calosc = calosc + tokens[i];
                        }
                    }
                }
                tokens[tokens.Length - 2] = tokens[tokens.Length - 2].Replace(".", string.Empty);
                calosc = calosc + "|" + tokens[tokens.Length - 2] + "|" + tokens[tokens.Length-1];
                File.AppendAllText("csv.txt", calosc + Environment.NewLine);
                Console.WriteLine(calosc);
            }
            Console.ReadKey();
        }
    }
}
