using System;
using System.Collections.Generic;
using System.IO;

namespace Pattern
{
    //Zasada Liskov


    class Program
    {
        static void Main(string[] args)
        {
            var rc = new Rectangle(2, 3);
            UseIt(rc);

            var sq = new Square(5);
            UseIt(sq);

            Console.ReadKey();
        }

        public static void UseIt(Rectangle r)
        {
            r.Height = 10;
            Console.WriteLine($"Oczekiwane pole powierzchni {10 * r.Width}, uzyskano {r.Area}");
        }
    }

    public class Rectangle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle() { }
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Area => Width * Height;
    }

    public class Square : Rectangle
    {
        public Square(int side)
        {
            Width = Height = side;
        }

        public new int Width
        {
            set { base.Width = base.Height = value; }
        }

        public new int Height
        {
            set { base.Width = base.Height = value; }
        }
    }
}
