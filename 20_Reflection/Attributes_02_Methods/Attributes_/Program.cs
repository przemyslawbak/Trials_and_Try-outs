using System;
using System.Linq;
using System.Runtime.CompilerServices;

[My(15)]
class Program
{
    [My(666)]
    static void Main(string[] args)
    {
        var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
        .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
        .Where(x => x.IsClass) // only yields classes
        .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
        .Where(x => x.GetCustomAttributes(typeof(MyAttribute), false).FirstOrDefault() != null); // returns only methods that have the InvokeAttribute

        foreach (var a in methods)
        {
            Console.WriteLine(a.Name);
        }

        Console.ReadKey();
    }

    [My(1)]
    public void MyMethod1(int a)
    {
        a = 1;

        Console.WriteLine("TestClass1->Method1");
    }

    [My(2)]
    public void MyMethod2(int b)
    {
        b = 2;

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
    }

    public int Value { get; private set; }
}