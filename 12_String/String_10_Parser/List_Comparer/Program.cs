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
            List<string> output = new List<string>();
            List<string> genericCities = File.ReadAllLines("1.txt").ToList();
            string[] addresses = File.ReadAllLines("2.txt");

            int i = 0;

            foreach(string address in addresses)
            {
                i++;

                Console.WriteLine(i + " z " + addresses.Count());
                string tocompare = genericCities.Where(c => c.ToLower().Trim() == address.ToLower().Trim()).FirstOrDefault();

                if (!string.IsNullOrEmpty(tocompare))
                {
                    output.Add(tocompare);
                }
                else
                {
                    output.Add("xxx");
                }
            }

            File.WriteAllLines("output.txt", output);

            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
