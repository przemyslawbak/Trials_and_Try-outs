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
                string[] linki = File.ReadAllLines("linki.txt");
            List<string> linkiFirm = new List<string>(linki);
            foreach (string nazwa in linkiFirm)
            {
                string[] tokens = nazwa.Split('/');
                Console.WriteLine(tokens[0]);
            }
            Console.ReadKey();
        }
    }
}
