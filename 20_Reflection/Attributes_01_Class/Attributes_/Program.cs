using System;
using System.Linq;
[My(15)]

//https://stackoverflow.com/a/1168659/11972985
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Program started");
        var ats =
            from a in typeof(Program).GetCustomAttributes(typeof(MyAttribute), true)
            let a2 = a as MyAttribute
            where a2 != null
            select a2;

        foreach (var a in ats)
            Console.WriteLine(a.Value);

        Console.WriteLine("Program ended");
        Console.ReadKey();
    }
}

[AttributeUsage(validOn: AttributeTargets.Class)]
public class MyAttribute : Attribute
{
    public MyAttribute(int x)
    {
        Console.WriteLine("MyAttribute created with {0}.", x);
        Value = x;
    }

    public int Value { get; private set; }
}