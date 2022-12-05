using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stara_lista = File.ReadAllLines("stara_lista.txt");
            string[] nowa_lista = File.ReadAllLines("nowa_lista.txt");
            List<string> pierwotnaLista = new List<string>(stara_lista);
            List<string> pozniejszaLista = new List<string>(nowa_lista);

            List<string> except = pozniejszaLista.Except(pierwotnaLista).ToList();
            for (var i = 0; i < except.Count; i++)
            {
                Console.WriteLine(except[i]);
            }
            Console.ReadKey();

        }
    }
}
