using System;

namespace Unit_Test_09___KStafford
{
    public interface IStore //IStore defines Save method, for production we create Store class with Save method
    {
        void Save(int result);
    }
    public class StringCalculator
    {
        private readonly IStore _store;
        public StringCalculator(IStore store)
        {
            _store = store;
        }
        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            var numbers = input.Split(',');
            var total = 0;
            foreach (var number in numbers)
            {
                total += TryparseToInteger(number);
            }
            if (_store != null)
            {
                if(IsPrime(total))
                {
                    _store.Save(total); //save
                }
            }
            return total;
        }
        private bool IsPrime(int number)
        {
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            for (int i = 3; i <= (int)(Math.Sqrt(number)); i +=2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private int TryparseToInteger(string input)
        {
            int dest;
            if (!int.TryParse(input, out dest))
            {
                throw new ArgumentException("Input format incorrect");
            }
            return dest;
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
