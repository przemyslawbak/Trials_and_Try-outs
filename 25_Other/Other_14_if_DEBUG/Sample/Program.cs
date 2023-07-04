namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG
            Console.WriteLine("THE BUG");
            #else
            Console.WriteLine("NOT THE BUG");
            #endif
        }
    }
}



