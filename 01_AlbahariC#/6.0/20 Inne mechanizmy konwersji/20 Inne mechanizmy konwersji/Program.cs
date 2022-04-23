using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace _20_Inne_mechanizmy_konwersji
{
    class Program
    {
        static void Main(string[] args)
        {
            //BitConverter
            foreach (byte b in BitConverter.GetBytes(666))
                Console.Write(b + " "); // 0 0 0 0 0 0 12 64
            //XMLConverter
            string s = XmlConvert.ToString(true); // s = "true"
            bool isTrue = XmlConvert.ToBoolean(s);
            //Color
            TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
            Color beige = (Color)cc.ConvertFromString("Beige");
            Color purple = (Color)cc.ConvertFromString("#800080");
            Console.WriteLine("\n" + beige);
            Console.WriteLine(purple.Name);
            //ChangeType
            Type targetType = typeof(int);
            object source = "42";
            object result = Convert.ChangeType(source, targetType);
            Console.WriteLine(result); // 42
            Console.WriteLine(result.GetType()); // System.Int32
            //Convert
            double d = 3.9;
            int i = Convert.ToInt32(d); // i == 4
            uint five = Convert.ToUInt32("101", 2); // parsowanie w systemie binarnym
            Console.ReadKey();
        }
    }
}
