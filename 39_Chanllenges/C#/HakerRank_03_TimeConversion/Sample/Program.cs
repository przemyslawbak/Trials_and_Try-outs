using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "07:05:45PM";

            DateTime t = DateTime.Parse(s);

            Console.WriteLine(t.ToString("HH:mm:ss")); //NOTE: in the challenge it has to return value

        }
    }
}
