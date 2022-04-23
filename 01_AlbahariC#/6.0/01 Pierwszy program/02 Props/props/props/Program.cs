using System;

namespace props
{
    class Program
    {
        static void Main(string[] args)
        {
            SomeClass some = new SomeClass();
            some.Dupa = true;

            Console.WriteLine(some.Dupa.ToString());

            Console.ReadKey();
        }
    }

    public class SomeClass
    {
        public bool Dupa { get; set; }
    }
}
