using System;
using System.Dynamic;

namespace _16_Typy_anonimowe //i wiązania dynamiczne
{
    class Program
    {
        static void Main(string[] args)
        {
            //wyrażenia są prawdziwe:
            //typeof (dynamic) == typeof (object)
            //typeof (List<dynamic>) == typeof (List<object>)
            //typeof (dynamic[]) == typeof (object[])
            //dynamic
            dynamic x = "cześć";
            Console.WriteLine(x.GetType().Name); // String
            Console.WriteLine(x); // cześć
            x = 123; // nie ma błędu (mimo tej samej zmiennej)
            Console.WriteLine(x); // 123
            Console.WriteLine(x.GetType().Name); // Int32

            //wykorzystanie dynamicznego wiązania
            dynamic d = new Duck();
            d.Quack(); // "Wywołano metodę Quack"
            d.Waddle(); // "Wywołano metodę Waddle"
            //typ anonimowy
            var dude = new { Name = "Bartek", Age = 23 }; //typ anonimowy utworzony w locie
            Console.WriteLine(dude);
            Console.WriteLine(dude.Age);
            Console.ReadKey();
        }
        //wiązanie niestandardowe
        public class Duck : DynamicObject
        {
            public override bool TryInvokeMember(
            InvokeMemberBinder binder, object[] args, out object result)
            {
                Console.WriteLine("Wywołano metodę " + binder.Name);
                result = null;
                return true;
            }
        }
    }
}
