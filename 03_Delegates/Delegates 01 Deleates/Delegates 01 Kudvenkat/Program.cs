using System;

namespace Delegates_01_Kudvenkat
{
    //Metoda musi mieć zgodną sygnaturę, co oznacza że musi przyjmować takie same argumenty i zwracać taki sam typ, jaki podaliśmy w deklaracji delegaty
    delegate void HelloFunctionDelegate(string Msg);
    delegate void Operation(int number);
    class Program
    {
        public static void Hello(string message)
        {
            Console.WriteLine(message);
        }
        static void Main(string[] args)
        {
            HelloFunctionDelegate del = new HelloFunctionDelegate(Hello);
            del("dupa");
            Test(del);
            Console.ReadKey();
            Operation op = Double;
            op += Triple; //chaining functions
            ExecuteOperation(2, op);
            Console.ReadKey();
        }
        static void Test (HelloFunctionDelegate del)
        {
            del("aaa");
        }
        static void Double(int num)
        {
            Console.WriteLine("{0} x 2 = {1}", num, num * 2);
        }
        static void Triple(int num)
        {
            Console.WriteLine("{0} x 3 = {1}", num, num * 3);
        }
        static void ExecuteOperation(int num, Operation operation)
        {
            operation(num);
        }
    }
}
