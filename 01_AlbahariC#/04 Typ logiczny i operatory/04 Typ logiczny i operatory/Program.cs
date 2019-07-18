using System;

namespace _04_Typ_logiczny_i_operatory
{
    public class Dude
    {
        public string Name;
        public Dude(string n) { Name = n; }
    }
    class Program
    {
        static bool UseUmbrella(bool rainy, bool sunny, bool windy)
        {
            return !windy && (rainy || sunny);
        }
        static void Main(string[] args)
        {
            //wartości
            int x = 1;
            int y = 2;
            int z = 1;
            Console.WriteLine(x == y); // fałsz
            Console.WriteLine(x == z); // prawda

            //referencje
            Dude d1 = new Dude("Jan");
            Dude d2 = new Dude("Jan");
            Console.WriteLine(d1 == d2); // fałsz
            Dude d3 = d1;
            Console.WriteLine(d1 == d3); // prawda

            //warunkowy
            Console.WriteLine("parasol?");
            Console.WriteLine(UseUmbrella(true, false, false)); //true
            Console.ReadKey();
        }
    }
}
