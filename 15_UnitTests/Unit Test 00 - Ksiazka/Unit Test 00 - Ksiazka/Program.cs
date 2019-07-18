using System;

namespace Unit_Test_00___Ksiazka
{
    //jak może wyglądać taki sposób pisania testów bez frameworka.
    public class SimpleParser
    {
        public SimpleParser()
        {

        }
        public int ParseAndSum(string numbers)
        {
            if (numbers.Length == 0)
            {
                return 0;
            }
            if (!numbers.Contains(","))
            {
                return int.Parse(numbers);
            }
            else
            {
                throw new InvalidOperationException(
                "Na razie potrafię obsłużyć tylko 0 lub 1 liczbę!");
            }
        }
    }
    class SimpleParserTests
    {
        public static void TestReturnsZeroWhenEmptyString()
        {
            try
            {
                SimpleParser p = new SimpleParser();
                int result = p.ParseAndSum(string.Empty);
                if (result != 0)
                {
                    Console.WriteLine(
                    @"***SimpleParserTests.TestReturnsZeroWhenEmptyString:
                    -------
                    Metoda ParseAndSum powinna zwrócić 0 w przypadku przekazania
                    pustego ciągu znaków
                    ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            SimpleParser p = new SimpleParser();
            int parsed = p.ParseAndSum("4");
            Console.WriteLine(parsed);
            try
            {
                SimpleParserTests.TestReturnsZeroWhenEmptyString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
