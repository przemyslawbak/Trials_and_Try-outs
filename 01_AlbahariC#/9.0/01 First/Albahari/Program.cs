
using System;

namespace Albahari
{
    class Program
    {
        static void Main(string[] args)
        {
            //Przedstawiamy program wykonujący mnożenie wartości 12 przez 30 i drukujący wynik, 360
            int x = 12 * 30; // instrukcja 1
            System.Console.WriteLine(x); // instrukcja 2

            Console.WriteLine(FeetToInches(30)); // 360
            Console.WriteLine(FeetToInches(100)); // 1200
            int FeetToInches(int feet)
            {
                int inches = feet * 12;
                return inches;
            }

            //słowa zarezerwowane
            //int using = 123; // źle
            int @using = 123; // dobrze

            Panda p1 = new Panda("Pan Dee");
            Panda p2 = new Panda("Pan Dah");
            Console.WriteLine(p1.Name); // Pan Dee
            Console.WriteLine(p2.Name); // Pan Dah
            Console.WriteLine(Panda.Population); // 2
        }

    }

    public class Panda
    {
        public string Name; // Instance field
        public static int Population; // Static field
        public Panda(string n) // Constructor
        {
            Name = n; // Assign the instance field
            Population = Population + 1; // Increment the static Population field
        }
    }
}
