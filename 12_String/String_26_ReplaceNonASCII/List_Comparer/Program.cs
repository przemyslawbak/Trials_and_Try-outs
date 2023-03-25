using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] lista = File.ReadAllLines("1.txt");
            List<string> pierwotnaLista = new List<string>(lista);
            pierwotnaLista = pierwotnaLista.Select(word => RemoveDiacritics(word)).ToList();
            pierwotnaLista = pierwotnaLista.Select(word => GetIso(word)).ToList();

            var result = pierwotnaLista.GroupBy(test => test).Select(grp => grp.First()).ToList();

            System.IO.File.WriteAllLines("output2.txt", result);
            Console.WriteLine("saved");
            Console.ReadKey();

        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        static string GetIso(string text)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

            return asciiStr;
        }
    }
}
