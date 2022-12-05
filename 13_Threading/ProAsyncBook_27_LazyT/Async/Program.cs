using System;
using System.Threading;

namespace Async
{
    //Listing 5-8. Simple Use of Lazy<T>
    //person object doesn’t get created until the Value is requested

    class Program
    {
        static void Main(string[] args)
        {
            Lazy<Person> lazyPerson = new Lazy<Person>();
            Console.WriteLine("Lazy object created");
            Console.WriteLine("has person been created {0}", lazyPerson.IsValueCreated ? "Yes" : "No");
            Console.WriteLine("Setting Name");
            lazyPerson.Value.Name = "Andy"; // Creates the person object on fetching Value
            Console.WriteLine("Setting Age");
            lazyPerson.Value.Age = 21; // Re-uses same object from first call to Value
            Person andy = lazyPerson.Value;
            Console.WriteLine(andy);
        }
    }

    public class Person
    {
        public Person()
        {
            Thread.Sleep(5000);
            Console.WriteLine("Created");
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", Name, Age);
        }
    }
}
