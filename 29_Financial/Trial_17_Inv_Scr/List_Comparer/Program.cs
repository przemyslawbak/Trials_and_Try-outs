using System;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {

        static void Main(string[] args)
        {

            string text = File.ReadAllText("1.txt");
            var numbers = text.Split(',').Select(x => int.Parse(x)).ToList();
            var diffs = numbers.Select((x, i) => i == 0 ? 0 : x - numbers[i - 1]).ToList();
            var greaterDiffs = diffs.Where(x => x > 300).ToList();
            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
