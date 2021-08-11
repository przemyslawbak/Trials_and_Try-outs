using System;
using System.Collections.Generic;

namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize array of characters from a to z.
            char[] chars = new char[26];
            for (int ctr = 0; ctr < 26; ctr++)
                chars[ctr] = (char)(ctr + 0x0061);
            object obj = System.Activator.CreateInstance(typeof(string), new object[] { chars, 13, 10 });
            Console.WriteLine(obj);

            Model model = new Model()
            {
                TestProp = "dupa"
            };

            Dictionary<Type, object> someDict = new Dictionary<Type, object>();
            someDict.Add(typeof(Model), model);

            RecoverModel(someDict);
        }

        private static void RecoverModel(Dictionary<Type, object> someDict)
        {
            foreach (var item in someDict)
            {
                var value = Convert.ChangeType(item.Value, item.Key);
            }
        }
    }   
}
