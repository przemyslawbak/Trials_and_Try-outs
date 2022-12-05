using System;

namespace Delegates_02_FuncAction
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething(Function1, "Hell-o World!"); //calling action
            Console.ReadKey();
            Check(Checkup1, "Hi World!"); //false
            Check(Checkup2, "Hi World!"); //true
            Console.ReadKey();
            Func<int, int, string> tfunc = null;
            tfunc += Add; // bind first method
            tfunc += Add; // bind first method <- chaining Func
            Console.WriteLine(tfunc(2, 2));
            Console.ReadKey();
        }
        public static void Function1(string input)
        {
            Console.WriteLine(input);
        }
        public static void DoSomething(Action<string> action, string input) //action
        {
            action(input);
        }
        public static void Check(Func<string, bool> func, string input) //function
        {
            Console.WriteLine(func(input));
        }
        public static bool Checkup1(string input)
        {
            return input.Length > 10;
        }public static bool Checkup2(string input)
        {
            return input.Length < 10;
        }
        private static string Add(int a, int b)
        {
            Console.WriteLine("Inside Add");
            return "Add: " + (a + b).ToString();
        }
    }
}
