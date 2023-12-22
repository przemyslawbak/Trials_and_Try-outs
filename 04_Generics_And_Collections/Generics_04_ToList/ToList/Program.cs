using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list1 = new List<int>() { 1, 2, 3, 4, 5 };
            List<int> list2 = CreateNewList(list1.ToList());
            list1.Add(0);
            list2.Add(7);
            list1.Add(8);
            Console.WriteLine();
            Console.WriteLine("list1:");
            for (int i = 0; i < list1.Count; i++) Console.Write(list1[i]);
            Console.WriteLine();
            Console.WriteLine("list2:");
            for (int i = 0; i < list2.Count; i++) Console.Write(list2[i]);

            Console.ReadKey();
        }

        private static List<int> CreateNewList(List<int> list1)
        {
            List<int> list3 = list1;
            list3.Add(6);
            Console.WriteLine("list3:");
            for (int i = 0; i < list3.Count; i++) Console.Write(list3[i]);
            Console.WriteLine();
            Console.WriteLine("list1 copy:");
            for (int i = 0; i < list1.Count; i++) Console.Write(list1[i]);


            return list1;
        }
    }
}
