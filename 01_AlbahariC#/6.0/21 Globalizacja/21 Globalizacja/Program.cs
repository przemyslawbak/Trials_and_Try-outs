using System;
using System.Numerics;
using System.Security.Cryptography;

namespace _21_Globalizacja
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger googol = BigInteger.Parse("1".PadRight(100, '0'));
            double g2 = (double)googol; // rzutowanie jawne
            BigInteger g3 = (BigInteger)g2; // rzutowanie jawne
            Console.WriteLine(g3);
            // w tym kodzie używana jest przestrzeń nazw System.Security.Cryptography
            RandomNumberGenerator rand = RandomNumberGenerator.Create();
            byte[] bytes = new byte[32];
            rand.GetBytes(bytes);
            var bigRandomNumber = new BigInteger(bytes); // konwersja na typ BigInteger
            //Complex - liczby zespolone
            var c1 = new Complex(2, 3.5); //zespolone <- część rzeczywista i urojona
            var c2 = new Complex(3, 0);
            Console.WriteLine(c1.Real); // 2
            Console.WriteLine(c1.Imaginary); // 3,5
            Console.WriteLine(c1.Phase); // 1,05165021254837
            Console.WriteLine(c1.Magnitude); // 4,03112887414927
            //Random
            Random r1 = new Random(1); //w nawiasie ziarno
            Random r2 = new Random(1);
            Console.WriteLine(r1.Next(100) + ", " + r1.Next(100)); // 24, 11
            Console.WriteLine(r2.Next(100) + ", " + r2.Next(100)); // 24, 11
            Console.ReadKey();
        }
    }
}
