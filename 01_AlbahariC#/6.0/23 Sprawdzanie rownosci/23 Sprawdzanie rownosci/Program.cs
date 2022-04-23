using System;
using System.Text;

namespace _23_Sprawdzanie_rownosci
{
    //IEquatable implementacja
    public struct Area : IEquatable<Area>
    {
        public readonly int Measure1;
        public readonly int Measure2;
        public Area(int m1, int m2)
        {
            Measure1 = Math.Min(m1, m2);
            Measure2 = Math.Max(m1, m2);
        }
        public override bool Equals(object other)
        {
            if (!(other is Area)) return false;
            return Equals((Area)other); // wywołuje poniższą metodę
        }
        public bool Equals(Area other) // implementuje IEquatable<Area>
        => Measure1 == other.Measure1 && Measure2 == other.Measure2;
        public override int GetHashCode()
        => Measure2 * 31 + Measure1; // 31 = jakaś liczba pierwsza
        public static bool operator ==(Area a1, Area a2) => a1.Equals(a2);
        public static bool operator !=(Area a1, Area a2) => !a1.Equals(a2);
    }
    class Program
    {
        static void Main(string[] args)
        {
            Area a1 = new Area(5, 10);
            Area a2 = new Area(10, 5);
            Console.WriteLine(a1.Equals(a2)); // prawda
            Console.WriteLine(a1 == a2); // prawda
            //
            int x = 5, y = 5;
            Console.WriteLine(x == y); // prawda (dzięki zasadom równości wartościowej)
            var dt1 = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.FromHours(8));
            var dt2 = new DateTimeOffset(2010, 1, 1, 2, 1, 1, TimeSpan.FromHours(9));
            Console.WriteLine(dt1 == dt2); // True
            Uri uri1 = new Uri("http://www.linqpad.net");
            Uri uri2 = new Uri("http://www.linqpad.net");
            Console.WriteLine(uri1 == uri2); // prawda
            double z = double.NaN;
            Console.WriteLine(z == z); // fałsz
            Console.WriteLine(z.Equals(z)); // prawda
            var sb1 = new StringBuilder("foo");
            var sb2 = new StringBuilder("foo");
            Console.WriteLine(sb1 == sb2); // fałsz (równość referencyjna)
            Console.WriteLine(sb1.Equals(sb2)); // prawda (równość wartościowa)
            Console.ReadKey();
        }


    }
}
