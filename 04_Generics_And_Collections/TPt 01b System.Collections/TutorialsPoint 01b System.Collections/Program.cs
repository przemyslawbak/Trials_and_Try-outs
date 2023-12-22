using System;
using System.Collections;

namespace TutorialsPoint_01b_System.Collections
{
    class Program //https://www.tutorialspoint.com/csharp/csharp_hashtable.htm
    {
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();

            ht.Add("01", "test1");
            ht.Add("02", "test2");
            ht.Add("03", "test3");
            ht.Add("04", "test4");
            ht.Add("05", "test5");
            ht.Add("06", "test6");

            if (ht.ContainsValue("test1"))
            {
                Console.WriteLine(ht["01"]);
            }

            // Get a collection of the keys.
            ICollection key = ht.Keys;

            foreach (string k in key)
            {

                Console.WriteLine(k + ": " + ht[k]);

                Console.ReadKey();
            }

            Console.WriteLine("test " + ht["01"]);

            Console.ReadKey();
        }


    }
}
