using System;

namespace LogAn
{
    //Wstrzykiwanie sztucznego obiektu poprzez dodanie
    //setterów właściwości do testowanej klasy
    public class LogAnalyzer
    {
        private IExtensionManager manager;
        public LogAnalyzer()
        {
            manager = new FileExtensionManager();
        }
        public IExtensionManager ExtensionManager //Umożliwienie ustawienia zależności za pomocą właściwości
        {
            get { return manager; }
            set { manager = value; }
        }
        public bool IsValidLogFileNameee(string fileName)
        {
            return manager.IsValid(fileName); //wykorzystanie managera
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
