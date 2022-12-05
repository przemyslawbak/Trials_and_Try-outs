using System;
using System.Globalization;
using System.Text;

namespace _19_Formatowanie_i_parsowanie
{
    class Program
    {
        static void Main(string[] args)
        {
            //formatowanie i parsowanie
            string s = true.ToString(); // s = "True"
            bool b = bool.Parse(s); // b = True
            int i;
            bool failure = int.TryParse("qwerty", out i);
            bool success = int.TryParse("123", out i);
            Console.WriteLine(double.Parse("1,234"));
            double x = double.Parse("1.234", CultureInfo.InvariantCulture); //qlturka
            string y = 1.234.ToString(CultureInfo.InvariantCulture);
            //z dostawcą formatu
            NumberFormatInfo f = new NumberFormatInfo();
            f.CurrencySymbol = "$$";
            Console.WriteLine(3.ToString("C", f)); // $$ 3.00 <-currency + format
            //C format
            Console.WriteLine(10.3.ToString("C")); // 10,30 zł
            Console.WriteLine(10.3.ToString("F4")); // 10,3000 (dokładność do czterech miejsc po przecinku)
            //culture info
            CultureInfo uk = CultureInfo.GetCultureInfo("en-GB");
            Console.WriteLine(3.ToString("C", uk)); // £3.00
            //datetime format
            DateTime dt = new DateTime(2000, 1, 2);
            CultureInfo iv = CultureInfo.InvariantCulture;
            Console.WriteLine(dt.ToString(iv)); // 01/02/2000 00 00 00
            Console.WriteLine(dt.ToString("d", iv)); // 01/02/2000
            //number format
            NumberFormatInfo z = new NumberFormatInfo();
            z.NumberGroupSeparator = " ";
            Console.WriteLine(12345.6789.ToString("N3", z)); // 12 345.679
            //użycie IFormatProvider, ICustomFormatter
            double n = -123.45;
            IFormatProvider fp = new WordyFormatProvider();
            Console.WriteLine(string.Format(fp, "{0:C} słownie to {0:W}", n));
            // –123.45 zł słownie to minus jeden dwa trzy kropka cztery pięć
            Console.ReadKey();
        }
    }
    public interface IFormattable
    {
        string ToString(string format, IFormatProvider formatProvider);
    }
    //ICustomFormatter
    public class WordyFormatProvider : IFormatProvider, ICustomFormatter
    {
        static readonly string[] _numberWords =
        "zero jeden dwa trzy cztery pięć sześć siedem osiem dziewięć minus przecinek".Split();
        IFormatProvider _parent; // umożliwia konsumentom łączenie dostawców formatu w łańcuchy
        public WordyFormatProvider() : this(CultureInfo.CurrentCulture) { }
        public WordyFormatProvider(IFormatProvider parent)
        {
            _parent = parent;
        }
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }
        public string Format(string format, object arg, IFormatProvider prov)
        {
            // jeśli to nie jest nasz łańcuch formatu, oddajemy zadanie do dostawcy nadrzędnego
            if (arg == null || format != "W")
                return string.Format(_parent, "{0:" + format + "}", arg);
            StringBuilder result = new StringBuilder();
            string digitList = string.Format(CultureInfo.InvariantCulture, "{0}", arg);
            foreach (char digit in digitList)
            {
                int i = "0123456789-.".IndexOf(digit);
                if (i == -1) continue;
                if (result.Length > 0) result.Append(' ');
                result.Append(_numberWords[i]);
            }
            return result.ToString();
        }
    }
}
