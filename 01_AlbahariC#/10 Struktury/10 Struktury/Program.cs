using System;

namespace _10_Struktury
{

    //interfejs
    public interface IEnumerator
    {
        bool MoveNext();
        object Current { get; }
        void Reset();
    }
    internal class Countdown : IEnumerator
    {
        int count = 11;
        public bool MoveNext() => count-- > 0;
        public object Current => count;
        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
    //struktura
    public struct Point
    {
        int x, y;
        //Jeśli programista zdefiniuje konstruktor struktury, musi jawnie przypisać wartość każdemu polu.
        public Point(int x, int y)
        {
            this.x = x; this.y = y;
        }
    }
    //jawna impementacja interfejsu
    interface I1
    {
        void Foo();
    }
    interface I2
    {
        int Foo();
    }
    public class Widget : I1, I2
    {
        public void Foo()
        {
            Console.WriteLine("Implementacja składowej I1.Foo w klasie Widget.");
        }
        int I2.Foo()
        {
            Console.WriteLine("Implementacja składowej I2.Foo w klasie Widget.");
            return 42;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //rzutowanie
            Widget w = new Widget();
            w.Foo(); // Implementacja metody I1.Foo z klasy Widget.
            ((I1)w).Foo(); // Implementacja metody I1.Foo z klasy Widget.
            ((I2)w).Foo(); // Implementacja metody I2.Foo z klasy Widget.
            Console.ReadKey();

            //struktura
            Point p1 = new Point(); // p1.x i p1.y będą miały wartość 0
            Point p2 = new Point(1, 1); // p1.x i p1.y będą miały wartość 1

            //interfejs
            IEnumerator e = new Countdown(); //niejawne rzutowanie
            while (e.MoveNext())
                Console.Write(e.Current); // 109876543210
        }
    }
}
