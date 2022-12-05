using System;

namespace Pattern_
{
    //Singleton: https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/
    //Singleton: https://en.wikibooks.org/wiki/Computer_Science_Design_Patterns/Singleton
    class Program
    {
        static void Main(string[] args)
        {
            Singleton.Instance.ValueOne = 10.5;
            Singleton.Instance.ValueTwo = 5.5;
            Console.WriteLine("Addition : " + Singleton.Instance.Addition());
            Console.WriteLine("Subtraction : " + Singleton.Instance.Subtraction());
            Console.WriteLine("Multiplication : " + Singleton.Instance.Multiplication());
            Console.WriteLine("Division : " + Singleton.Instance.Division());
            Console.WriteLine("\n----------------------\n");
            Singleton.Instance.ValueTwo = 10.5;
            Console.WriteLine("Addition : " + Singleton.Instance.Addition());
            Console.WriteLine("Subtraction : " + Singleton.Instance.Subtraction());
            Console.WriteLine("Multiplication : " + Singleton.Instance.Multiplication());
            Console.WriteLine("Division : " + Singleton.Instance.Division());

            // Wait for user
            Console.ReadKey();
        }
    }

    //calc example
    public sealed class Singleton
    {
        public static Singleton Instance
        {
            get { return _lazyInstance.Value; }
        }
        //Use Lazy<T> to lazily initialize the class and provide thread-safe access
        private static readonly Lazy<Singleton> _lazyInstance = new Lazy<Singleton>(() => new Singleton());

        public double ValueOne { get; set; }
        public double ValueTwo { get; set; }

        public double Addition()
        {
            return ValueOne + ValueTwo;
        }
        public double Subtraction()
        {
            return ValueOne - ValueTwo;
        }
        public double Multiplication()
        {
            return ValueOne * ValueTwo;
        }
        public double Division()
        {
            return ValueOne / ValueTwo;
        }
    }
}
