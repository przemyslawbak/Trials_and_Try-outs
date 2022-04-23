using System;
using System.Text;

namespace _17_Obsluga_lancuchow_i_tekstu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Char.ToUpper('c')); // C
            Console.WriteLine(char.IsWhiteSpace('\t')); // prawda
            //tworzenie łańcuchów
            string s1 = "Cześć";
            string s2 = "Pierwszy wiersz\r\nDrugi wiersz";
            string s3 = @"\\server\fileshare\helloworld.cs";
            Console.Write(new string('*', 10)); // **********
            //tablica char
            char[] ca = "Cześć".ToCharArray();
            //Pobieranie znaków z łańcucha
            string str = "abcde";
            char letter = str[1]; // letter == 'b'
            Console.WriteLine(letter);
            //przeszukiwanie łańcuchów
            Console.WriteLine("gdyby kózka nie skakała".EndsWith("skakała")); // true
            Console.WriteLine("gdyby kózka nie skakała".Contains("kózka")); // false
            //Metoda IndexOfAny zwraca pozycję pierwszego znalezionego elementu z podanego zbioru znaków
            Console.Write("ab,cd ef".IndexOfAny(new char[] { ' ', ',' })); // 2 (miejsce przecinka)
            Console.Write("pas5w0rd".IndexOfAny("0123456789".ToCharArray())); // 3 (miejsce cyfry)
            //Metoda Substring pobiera część łańcucha:
            string left3 = "12345".Substring(0, 3); // left3 = "123";
            string mid3 = "12345".Substring(1, 3); // mid3 = "234";
            //wstawienie
            string s5 = "witajświecie".Insert(5, ", "); // s1 = "witaj, świecie"
            //Metody PadLeft i PadRight dopełniają łańcuch do określonej długości przez dodanie odpowiedniej liczby egzemplarzy wskazanego znaku
            Console.WriteLine("12345".PadLeft(9, '*')); // ****12345
            Console.WriteLine("12345".PadLeft(9)); // 12345
            //Metody TrimStart i TrimEnd usuwają określone znaki z początku i końca łańcucha
            Console.WriteLine(" abc \t\r\n ".Trim().Length); // 3
            //Metoda Replace zamienia wszystkie (niepokrywające się) egzemplarze wskazanego znaku lub łańcucha
            Console.WriteLine("co tu robić".Replace(" ", " | ")); // co | tu | robić
            Console.WriteLine("co tu robić".Replace(" ", "")); // coturobić
            //StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 50; i++) sb.Append(i + ",");
            sb = sb.Append(" ja sem stringbuilder");
            Console.WriteLine(sb);
            Console.ReadKey();
        }
    }
}