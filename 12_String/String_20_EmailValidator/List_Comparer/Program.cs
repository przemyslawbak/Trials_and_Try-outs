﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            string theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                      + "@"
                                      + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            string regexAscii = @"[^\u0000-\u007F]+";

            string[] lista = File.ReadAllLines("1.txt");
            List<string> pierwotnaLista = new List<string>(lista);

            List<string> output = new List<string>();
            foreach (var email in pierwotnaLista)
            {
                if (Regex.IsMatch(email, theEmailPattern) && !Regex.IsMatch(email, regexAscii))
                {
                    output.Add(email);
                    Console.WriteLine(email);
                }
                else
                {
                    //do not add
                }



            }
            File.WriteAllLines("output.txt", output.OrderBy(q => q).ToList());
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
