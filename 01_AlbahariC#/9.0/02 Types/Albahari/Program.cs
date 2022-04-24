
using System;

namespace Albahari
{
    public class Dude
    {
        public string Name;
        public Dude(string n) { Name = n; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Typy wartościowe
            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1; // przypisanie powoduje utworzenie kopii
            Console.WriteLine(p1.X); // 7
            Console.WriteLine(p2.X); // 7
            p1.X = 9; // zmiana p1.X
            Console.WriteLine(p1.X); // 9
            Console.WriteLine(p2.X); // 7

            //Typy referencyjne
            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1; // kopiuje referencję p1
            Console.WriteLine(p1.X); // 7
            Console.WriteLine(p2.X); // 7
            p1.X = 9; // zmienia p1.X
            Console.WriteLine(p1.X); // 9
            Console.WriteLine(p2.X); // 9

            //Wartość null - referencyjny
            Point p = null;
            Console.WriteLine(p == null); // prawda

            //Wartość null - wartościowy
            Point p = null; // błąd kompilacji
            int x = null; // błąd kompilacji'

            //są też nullable types

            /*
             * W przypadku typów referencyjnych równość jest standardowo określana na podstawie referencji, a nie rzeczywistej wartości obiektu
            */
            Dude d1 = new Dude("Jan");
            Dude d2 = new Dude("Jan");
            Console.WriteLine(d1 == d2); // fałsz
            Dude d3 = d1;
            Console.WriteLine(d1 == d3); // prawda

            //string
            string a = "test";
            string b = "test";
            Console.Write(a == b); // prawda
            string escaped = "Pierwszy wiersz\r\nDrugi wiersz";
            string verbatim = @"Pierwszy wiersz
Drugi wiersz";
            // Prawda, jeśli środowisko IDE używa znaku nowego wiersza CR-LF:
            Console.WriteLine(escaped == verbatim);

            string s = "a" + "b";
            string t = "a" + 5; // a5
            int x = 4;
            Console.Write($"Kwadrat ma {x} boki."); // drukuje: Kwadrat ma 4 boki.
            string s = $"255 w systemie szesnastkowym to {byte.MaxValue:X2}"; // X2 = cyfra szesnastkowa. Wynik: 255 w systemie szesnastkowym to FF"

        }

    }
}
