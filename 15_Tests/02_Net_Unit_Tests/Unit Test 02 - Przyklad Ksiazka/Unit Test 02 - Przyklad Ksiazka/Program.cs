using System;
using System.Reflection;

namespace Unit_Test_02___Przyklad_Ksiazka
{
    //bez frameworka XUnit
    class SimpleParserTests
    {
        public class TestUtil
        {
            public static void ShowProblem(string test, string message)
            {
                string msg = string.Format(@"
                                            ---{0}---
                                            {1}
                                            --------------------
                                            ", test, message);
                Console.WriteLine(msg);
            }
        }
        public static void TestReturnsZeroWhenEmptyString()
        {
            //wykorzystanie API refleksji frameworka .NET
            // do uzyskania bieżącej nazwy metody
            // można by ją zakodować na sztywno,
            //ale posługiwanie się refleksjami jest użyteczną techniką, którą warto znać
            string testName = MethodBase.GetCurrentMethod().Name;
            try
            {
                SimpleParser p = new SimpleParser();
                int result = p.ParseAndSum("1");
                if (result != 0)
                {
                    // Wywołanie metody pomocniczej
                    TestUtil.ShowProblem(testName,
                    "Metoda ParseAndSum powinna zwrócić 0 w przypadku przekazania pustego ciągu znaków");
                }
            }
            catch (Exception e)
            {
                TestUtil.ShowProblem(testName, e.ToString());
            }
        }
        public class SimpleParser
        {
            public int ParseAndSum(string numbers)
            {
                if (numbers.Length == 0) //jeśli string jest pusty
                {
                    return 0; //zwróć 0
                }
                if (!numbers.Contains(",")) //jeśli string zawiera ,
                {
                    return int.Parse(numbers); //parsuj string do int
                }
                else
                {
                    throw new InvalidOperationException(
                    "Na razie potrafię obsłużyć tylko 0 lub 1 liczbę!"); //wyjątek gdy nie jest pusty ani liczbą
                }
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
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
}
