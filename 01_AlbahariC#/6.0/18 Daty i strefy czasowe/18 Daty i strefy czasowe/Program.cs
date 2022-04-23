using System;

namespace _18_Daty_i_strefy_czasowe
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt1 = new DateTime(2015, 1, 1, 10, 20, 30, DateTimeKind.Local);
            DateTime dt2 = new DateTime(2015, 1, 1, 10, 20, 30, DateTimeKind.Utc);
            Console.WriteLine(dt1);
            Console.WriteLine(dt2);
            Console.WriteLine(dt1 == dt2); // prawda
            DateTime local = DateTime.Now;
            DateTime utc = local.ToUniversalTime();
            Console.WriteLine(local + " local");
            Console.WriteLine(utc + " utc");
            Console.WriteLine(local == utc); // fałsz
            DateTimeOffset locall = DateTimeOffset.Now;
            DateTimeOffset utcc = local.ToUniversalTime();
            Console.WriteLine(locall.Offset); // 01 00 00
            Console.WriteLine(utcc.Offset); // 00 00 00
            Console.WriteLine(local == utc); // prawda
            TimeZone zone = TimeZone.CurrentTimeZone;
            Console.WriteLine(zone.StandardName); // Środkowoeuropejski czas stand.
            Console.WriteLine(zone.DaylightName); // Środkowoeuropejski czas letni

            Console.ReadKey();
        }
    }
}
