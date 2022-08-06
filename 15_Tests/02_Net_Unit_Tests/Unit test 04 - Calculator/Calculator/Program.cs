using System;

namespace Calculator
{
    public class MemCalculator
    {
        private int sum = 0;
        public void Add(int number)
        {
            sum += number;
        }
        public int Sum()
        {
            int temp = sum;
            sum = 0;
            return temp;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
