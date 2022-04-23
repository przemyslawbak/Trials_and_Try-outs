using System;

namespace _22_Wyliczenia// i krotki i guid
{
    enum Nut { Walnut, Hazelnut, Macadamia } //wyliczenia
    enum Size { Small, Medium, Large } //wyliczenia
    class Program
    {
        static void Main(string[] args)
        {
            Display(Nut.Macadamia); // Nut.Macadamia
            Display(Size.Large); // Size.Large
        }
        static void Display(Enum value)
        {
            Console.WriteLine(value.GetType().Name + "." + value.ToString());
            //krotki
            var t = new Tuple<int, string>(123, "Cześć"); //utworzenie
            Tuple<int, string> u = Tuple.Create(123, "Cześć"); //utworzenie
            var w = Tuple.Create(123, "Cześć"); //utworzenie
            //porównywanie krotek
            var t1 = Tuple.Create(123, "Cześć");
            var t2 = Tuple.Create(123, "Cześć");
            Console.WriteLine(t1 == t2); // fałsz
            Console.WriteLine(t1.Equals(t2)); // prawda
            //guid
            Guid g = Guid.NewGuid();
            Console.WriteLine(g.ToString()); // 0d57629c-7d6e-4847-97cb-9e2fc25083fe
            Guid g1 = new Guid("{0d57629c-7d6e-4847-97cb-9e2fc25083fe}");
            Guid g2 = new Guid("0d57629c7d6e484797cb9e2fc25083fe");
            Console.WriteLine(g1 == g2); // True

            Console.ReadKey();
        }
    }
}
