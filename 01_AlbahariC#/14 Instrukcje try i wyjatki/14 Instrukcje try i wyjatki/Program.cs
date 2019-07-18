using System;
using System.IO;
using System.Net;

namespace _14_Instrukcje_try_i_wyjatki
{
    class Program
    {
        static void ReadFile()
        {
            //dispose bez względu na try
            StreamReader reader = null; // w przestrzeni nazw System.IO
            try
            {
                reader = File.OpenText("file.txt");
                if (reader.EndOfStream) return;
                Console.WriteLine(reader.ReadToEnd());
            }
            finally
            {
                if (reader != null) reader.Dispose(); //zamknęliśmy plik przez wywołanie metody Dispose obiektu klasy StreamReader
            }
        }
        static int Calc(int x) => 10 / x; //metoda dzielenia
        static void Main(string[] args)
        {
            string s = null;
            using (WebClient wc = new WebClient())
                try { s = wc.DownloadString("http://www.albahari.com/nutshell/"); } //pobiera html
                catch (WebException ex) //webexception
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                        Console.WriteLine("Timeout");
                    else
                        throw; // brak możliwości obsługi innych rodzajów wyjątków WebException, więc ponawiamy zgłoszenie
                }
            Console.WriteLine(s);
            Console.ReadKey();
            //wchodzi tylko pierwszy wyjatek i koniec, ew final
            try
            {
                byte b = byte.Parse(args[0]);
                Console.WriteLine(b);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Podaj przynajmniej jeden argument.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("To nie jest liczba!");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("Przekazano więcej niż jeden bajt!");
            }
            Console.ReadKey();
            //divide by zero
            try
            {
                int y = Calc(0); //dzielenie przez 0
                Console.WriteLine(y);
            }
            catch (DivideByZeroException ex) //exception
            {
                Console.WriteLine("x nie może mieć wartości zero.");
            }
            Console.WriteLine("Program zakończył działanie z powodzeniem.");
            Console.ReadKey();
        }
    }
}
