using System;

namespace Unit_Test_01___Console_app
{
    public class EuroCalculatorTests
    {
        public void Calculates_Pln()
        {
            decimal input = 50.0m;
            decimal euroRate = 4.35m;
            decimal correctResult = 217.5m;
            var calculator = new LocalEuroCalculator();
            var result = calculator.Calculate(input, euroRate);
            if (result != correctResult)
            {
                throw new Exception(string.Format("Expected {0} but actual result was {1}", correctResult, result));
            }
        }
    }
    public class LocalEuroCalculator
    {
        public decimal Calculate (decimal srcEur, decimal euroRate)
        {
            return srcEur * euroRate;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new EuroCalculatorTests();

            tests.Calculates_Pln();

            Console.WriteLine("All tests passed");

            Console.ReadKey();
        }
    }
}
