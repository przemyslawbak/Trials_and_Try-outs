using System;

namespace _05_Znaki
{
    class Program
    {
        static void Main(string[] args)
        {
            char c = 'A'; // pojedynczy znak
            Console.WriteLine(c);
            char copyrightSymbol = '\u00A9';
            char omegaSymbol = '\u03A9';
            char newLine = '\u000A';
            Console.WriteLine(copyrightSymbol);
            Console.WriteLine(omegaSymbol);
            Console.WriteLine(newLine);
            string a2 = @"\\server\fileshare\helloworld.cs";
            Console.WriteLine(a2);
            string escaped = "Pierwszy wiersz\r\nDrugi wiersz";
            string verbatim = @"Pierwszy wiersz
            Drugi wiersz";
            // Prawda, jeśli środowisko IDE używa znaku nowego wiersza CR-LF
            Console.WriteLine(escaped == verbatim);
            string s = "a" + "b" + "c";
            int x = 4;
            Console.WriteLine(s);
            Console.Write($"Kwadrat ma {x} boki."); // drukuje Kwadrat ma 4 boki.
            string t = $"255 w systemie szesnastkowym to {byte.MaxValue:X2}"; // X2 = cyfra szesnastkowa
                                                                              // Wynik 255 w systemie szesnastkowym to FF"
            Console.WriteLine(t);
            int w = 2;
            string u = $@"ten łańcuch zajmuje {
            w} wierszy";
            Console.WriteLine(u);
            Console.ReadKey();


        }
    }
}
