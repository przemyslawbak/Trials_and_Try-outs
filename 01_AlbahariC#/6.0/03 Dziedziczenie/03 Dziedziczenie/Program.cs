using System;

namespace _03_Dziedziczenie
{
    public class Baseclass
    {
        public int X;
        public Baseclass() { X = 1; }
    }

    public class Subclass : Baseclass
    {
        public Subclass() { Console.WriteLine(X); } // 1
    }

    public class Asset
    {
        public string Name;
        public virtual decimal Liability => 0; // własność wyrażeniowa
    }

    public class Stock : Asset // dziedziczy po Asset (nazwę)
    {
        public long SharesOwned;
    }
    public class House : Asset // dziedziczy po Asset (nazwę)
    {
        public decimal Mortgage;
        public override decimal Liability => Mortgage;
    }

    class Program
    {
        public static void Display(Asset asset)
        {
            System.Console.WriteLine(asset.Name);
        }
        static void Main(string[] args)
        {
            Stock msft = new Stock
            {
                Name = "MSFT", //po Asset
                SharesOwned = 1000
            };
            Console.WriteLine(msft.Name); // MSFT
            Console.WriteLine(msft.SharesOwned); // 1000
            House mansion = new House
            {
                Name = "Mansion", //po Asset
                Mortgage = 250000
            };
            Console.WriteLine(mansion.Name); // Mansion
            Console.WriteLine(mansion.Mortgage); // 250000

            Display(msft); //polimorfizm
            Display(mansion); //polimorfizm

            Console.WriteLine("rzutowanie w górę:");
            Asset a = msft; // rzutowanie w górę
            Console.WriteLine(a.Name);
            Stock s = (Stock)a; // rzutowanie w dół
            Console.WriteLine(s.SharesOwned); // nie ma błędu
            Console.WriteLine("virtual");
            House xxx = new House { Name = "McMansion", Mortgage = 250000 };
            Asset yyy = xxx;

            Console.WriteLine(mansion.Liability); // 250000
            Console.WriteLine(a.Liability); // 250000

            Console.ReadKey();
        }
    }
}
