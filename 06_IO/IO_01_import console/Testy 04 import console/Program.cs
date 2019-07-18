using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_04_import_console
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] separators = { ';' };
            string str = "";

            FileStream fs = new FileStream(@"c:\testy.txt",
                                           FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.Default);

            while ((str = sr.ReadLine()) != null)
            {
                Console.WriteLine(str);
            }

            Console.ReadLine();
        }
    }
}
