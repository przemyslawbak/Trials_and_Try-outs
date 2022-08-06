using System;

namespace LogAn
{
    //Wyodrębnienie interfejsu umożliwiającego
    //zastąpienie istniejącej implementacji
    public class LogAnalyzer
    {
        private IExtensionManager manager;
        //Wstrzyknięcie namiastki z wykorzystaniem konstruktora
        public LogAnalyzer(IExtensionManager mgr) //konstruktor, jak ASP.NET kontroler i repo
        {
            manager = mgr;
        }
        public bool IsValidLogFileNameee(string fileName)
        {
            return manager.IsValid(fileName); //wykorzystanie interfejsu
        }
        public interface IExtensionManager //Definicja nowego interfejsu
        {
            bool IsValid(string fileName);
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
