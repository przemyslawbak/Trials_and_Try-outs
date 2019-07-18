using System;

namespace _09_Typ_object
{
    public class Stack
    {
        int position;
        object[] data = new object[10];
        public void Push(object obj)
        {
            data[position++] = obj;
        }
        public object Pop()
        {
            return data[--position];
        }
    }
    public class Point { public int X, Y; }
    class Program
    {
        static void Main(string[] args)
        {
            Stack stack = new Stack();
            stack.Push("kiełbasa");
            stack.Push("salami");
            string s = (string)stack.Pop();
            Console.WriteLine(s); // kiełbasa

            //boxing unboxing
            int x = 9;
            object obj = x; // pakuje int
            int y = (int)obj; // rozpakowanie int

            //typeof i GetType
            Console.WriteLine("typeof i GetType");
            Point p = new Point();
            Console.WriteLine(p.GetType().Name); // Point
            Console.WriteLine(typeof(Point).Name); // Point
            Console.WriteLine(p.GetType() == typeof(Point)); // True
            Console.WriteLine(p.X.GetType().Name); // Int32
            Console.WriteLine(p.Y.GetType().FullName); // System.Int32
            Console.ReadKey();
        }
    }
}
