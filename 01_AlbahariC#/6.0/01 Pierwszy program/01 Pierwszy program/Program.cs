using System;

namespace _01_Pierwszy_program
{
    public class UnitConverter
    {
        int ratio; // pole
        public UnitConverter(int unitRatio)
        {
            ratio = unitRatio;
        } // konstruktor
        public int Convert(int unit)
        {
            return unit * ratio;
        } // metoda
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nPrzedstawiamy program wykonujący mnożenie wartości 12 przez 30 i drukujący wynik, 360");
            int x = 12 * 30;
            Console.WriteLine(x);
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nPowyższy program możemy więc poddać refaktoryzacji w celu zdefiniowania metody mnożącej dowolną liczbę całkowitą przez 12");
            Console.WriteLine(FeetToInches(30)); // 360
            Console.WriteLine(FeetToInches(100)); // 1200
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nPraca z łańcuchami polega na wywoływaniu na nich funkcji.");
            string message = "Witaj, świecie";
            string upperMessage = message.ToUpper();
            Console.WriteLine(upperMessage); // WITAJ, ŚWIECIE
            int y = 2015;
            message = message + y.ToString();
            Console.WriteLine(message); // Witaj, świecie2015
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nPredefiniowany typ bool reprezentuje tylko dwie potencjalne wartości: true (prawda) i false (fałsz).");
            bool simpleVar = false;
            if (simpleVar)
                Console.WriteLine("To nie będzie wydrukowane.");
            int z = 5000;
            bool lessThanAMile = z < 5280;
            if (lessThanAMile)
                Console.WriteLine("To zostanie wydrukowane.");
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nW poniższym przykładzie zdefiniowaliśmy własny typ o nazwie UnitConverter");
            UnitConverter feetToInchesConverter = new UnitConverter(12);
            UnitConverter milesToFeetConverter = new UnitConverter(5280);
            Console.WriteLine(feetToInchesConverter.Convert(30)); // 360
            Console.WriteLine(feetToInchesConverter.Convert(100)); // 1200
            Console.WriteLine(feetToInchesConverter.Convert(
            milesToFeetConverter.Convert(1))); // 63360
            Console.WriteLine("press any key...");
            Console.ReadKey();

            Console.WriteLine("\nPrzypisanie egzemplarza typu wartościowego zawsze powoduje jego skopiowanie.");
            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1; // przypisanie powoduje utworzenie kopii
            Console.WriteLine(p1.X); // 7
            Console.WriteLine(p2.X); // 7
            p1.X = 9; // zmiana p1.X
            Console.WriteLine(p1.X); // 9
            Console.WriteLine(p2.X); // 7
            Console.WriteLine("press any key...");
            Console.ReadKey();

        }
        public struct Point
        {
            public int X, Y;
        } //zdefiniowanie własnego typu wartościowego
        static int FeetToInches (int feet) // W tym przykładzie zdefiniowaliśmy metodę o nazwie FeetToInches
        {
            int inches = feet * 12;
            return inches;
        }
    }
}
