using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[My(15)]
class Program
{
    [My(25)]
    public string MyProperty { get; set; }
    [My(35)]
    public string SomeProperty { get; set; }

    [My(666)]
    static void Main(string[] args)
    {
        var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
        .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
        .Where(x => x.IsClass) // only yields classes
        .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
        .Where(x => x.GetCustomAttributes(typeof(MyAttribute), false).FirstOrDefault() != null); // returns only methods that have the MyAttribute

        var prop = typeof(Program).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .FirstOrDefault(p => p.GetCustomAttributes(typeof(MyAttribute), false).Count() == 1);

        foreach (var a in methods)
        {
            Console.WriteLine(a.Name + " from Main");
            var x = a.GetParameters();
            foreach (var i in x)
            {
                Console.WriteLine(i.Name);
            }
            var y = a.GetMethodBody();
        }

        Console.ReadKey();
    }

    [My(1)]
    public void MyMethod1(int propertyA)
    {
        propertyA = 1;
        MyProperty = "prop1";

        Console.WriteLine("TestClass1->Method1");
    }

    [My(2)]
    public void MyMethod2(int propertyB)
    {
        propertyB = 2;
        MyProperty = "prop2";

        Console.WriteLine("TestClass1->Method2");
    }
}

[AttributeUsage(AttributeTargets.All)]
public class MyAttribute : Attribute
{
    public MyAttribute(int x, [CallerMemberName] string memberName = "")
    {
        Console.WriteLine("MyAttribute created with {0}.", x);
        Value = x;
        Console.WriteLine(memberName + " from attribute");
    }

    public int Value { get; private set; }
}