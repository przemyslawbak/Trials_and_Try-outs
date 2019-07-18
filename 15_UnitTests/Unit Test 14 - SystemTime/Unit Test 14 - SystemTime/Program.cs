using System;

namespace Unit_Test_14___SystemTime
{
    public static class TimeLogger
    {
        //Kod produkcyjny korzystający z klasy SystemTime
        public static string CreateMessage(string info)
        {
            return SystemTime.Now.ToShortDateString() + " " + info;
        }
    }
    public class SystemTime
    {
        private static DateTime _date;
        //Klasa SystemTime umożliwia modyfikowanie bieżącego czasu
        public static void Set(DateTime custom)
        { _date = custom; }
        //Klasa SystemTime umożliwia resetowanie bieżącego czasu
        public static void Reset()
        { _date = DateTime.MinValue; }
        //Klasa SystemTime zwraca czas rzeczywisty albo czas sztuczny, o ile został ustawiony
        public static DateTime Now
        {
            get
            {
                if (_date != DateTime.MinValue)
                {
                    return _date;
                }
                return DateTime.Now;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
