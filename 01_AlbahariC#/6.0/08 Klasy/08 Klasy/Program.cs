using System;

namespace _08_Klasy
{
    //wlasnosci
    public class Stock
    {
        decimal currentPrice, sharesOwned; // prywatne pole pomocnicze
        public decimal CurrentPrice // własność publiczna
        {
            get { return currentPrice; }
            set { currentPrice = value; }
        }
        //wlasnosc tylko do odczytu
        public decimal Worth
        {
            get { return currentPrice * sharesOwned; }
        }
        //Inicjalizatory własności
        public decimal CosTam { get; set; } = 123;
    }
    //gostepnsc get set
    public class Foo
    {
        private decimal x;
        public decimal X
        {
            get { return x; }
            private set { x = Math.Round(value, 2); }
        }
    }

    class Octopus
    {
        //pola
        string name;
        public int Age = 10;
        static readonly int legs = 8,
            eyes = 2;
        //metody
        int Foo(int x)
        {
            return x * 2;
        }
        int Foo2(int x) => x * 2;
    }

    public class Panda
    {
        string name; // definicja pola
        public Panda(string n) // definicja konstruktora
        {
            name = n; // kod inicjalizacyjny (ustawienie wartości pola)
        }
    }
    //przeciążanie konstruktorów
    public class Wine
    {
        public decimal Price;
        public int Year;
        public Wine(decimal price) { Price = price; }
        public Wine(decimal price, int year) : this(price) { Year = year; }
    }
    //inicjalizacja pól
    class Player
    {
        int shields = 50; // pierwsza inicjalizacja
        int health = 100; // druga inicjalizacja
    }
    //inicjalizacja obiektu
    public class Bunny
    {
        public string Name;
        public bool LikesCarrots;
        public bool LikesHumans;
        public Bunny() { }
        public Bunny(string n) { Name = n; }
    }
    //referencja this
    public class Ktos
    {
        public Ktos Mate;
        public void Marry(Ktos partner)
        {
            Mate = partner;
            partner.Mate = this;
        }
    }
    //indeksator
    class Sentence
    {
        string[] words = "Nosił wilk razy kilka".Split();
        public string this[int wordNum] // indeksator
        {
            get { return words[wordNum]; }
            set { words[wordNum] = value; }
        }
    }
    //konstruktory statyczne
    class Foot
    {
        public static Foot Instance = new Foot();
        public static int X = 3;
        Foot() {
            Console.WriteLine(X);

            Console.WriteLine("konstr statyczny");
            Console.ReadKey();
        } // 0
    }
    class Program
    {

        public const string Message = "Witaj, świecie"; //stala
        public static double Circumference(double radius)
        {
            return 2 * System.Math.PI * radius;
        }
        static void Main(string[] args)
        {
            Console.WriteLine(Foot.X);


            Sentence l = new Sentence();
            Console.WriteLine(l[1]); // wilk
            l[1] = "kangur";
            Console.WriteLine(l[1]); // kangur
            Console.WriteLine(l);
            Console.ReadKey();

            // jeśli konstruktor nie ma parametrów, można opuścić pusty nawias
            Bunny b1 = new Bunny { Name = "Bo", LikesCarrots = true, LikesHumans = false };
            Bunny b2 = new Bunny("Bo") { LikesCarrots = true, LikesHumans = false };
            string s = "cześć";
            Console.WriteLine(s[0]); // 'c'
            Console.WriteLine(s[3]); // 'ś'
            string w = null;
            Console.WriteLine(w?[0]); // nic nie drukuje; brak błędu
            Console.ReadKey();
        }
    }
}
