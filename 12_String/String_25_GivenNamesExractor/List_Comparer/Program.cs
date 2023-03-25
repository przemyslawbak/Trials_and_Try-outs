using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        private static List<string> _givenNames = new List<string>();

        static void Main(string[] args)
        {
            foreach (string file in Directory.EnumerateFiles(Path.Combine("data"), "*.*"))
            {
                Console.WriteLine("Reading " + file);
                List<string> contents = File.ReadAllLines(file).ToList();
                List<string> names = contents.Where(c => c.Contains(",")).Select(i => i.Split(',')[0].Split(' ')[0].ToLower()).Select(name => RemoveAccents(name)).Where(name => name.Length >= 4).Distinct().ToList();
                File.AppendAllLines("output.txt", names.Where(name => !string.IsNullOrEmpty(name)));
            }

            Console.WriteLine("finished");
            Console.ReadKey();

        }

        public static string RemoveAccents(string input)
        {
            return new string(input
                .Normalize(System.Text.NormalizationForm.FormD)
                .ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }
    }
}
