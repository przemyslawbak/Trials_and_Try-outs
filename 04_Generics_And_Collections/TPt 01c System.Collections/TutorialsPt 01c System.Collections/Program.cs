using System;
using System.Collections;

namespace CollectionsApplication
{
    class Program //https://www.tutorialspoint.com/csharp/csharp_sortedlist.htm
    {
        static void Main(string[] args)
        {
            SortedList sl = new SortedList();

            sl.Add("003", "Zara Ali");
            sl.Add("002", "Abida Rehman");
            sl.Add("001", "Joe Holzner");
            sl.Add("004", "Mausam Benazir Nur");
            sl.Add("005", "M. Amlan");
            sl.Add("006", "M. Arif");
            sl.Add("007", "Ritesh Saikia");

            if (sl.ContainsValue("Nuha Ali"))
            {
                Console.WriteLine("This student name is already in the list");
            }
            else
            {
                sl.Add("008", "Nuha Ali");
            }

            // get a collection of the keys. 
            ICollection key = sl.Keys;

            Console.WriteLine("test " + sl["001"]);
            Console.WriteLine("test2 " + sl.GetByIndex(0));

            Console.ReadKey();
        }
    }
}