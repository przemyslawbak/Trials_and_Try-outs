using System;
using System.IO;

namespace LogAn
{
    //Wstrzykiwanie sztucznego obiektu poprzez dodanie
    //setterów właściwości do testowanej klasy
    public class LogAnalyzer
    {
        private IExtensionManager manager;
        public LogAnalyzer()
        {
            ExtensionManagerFactory manager = new ExtensionManagerFactory(); //Wykorzystanie fabryki w kodzie produkcyjnym
            manager.Create();
        }
        public bool IsValidLogFileNameee(string fileName)
        {
            return manager.IsValid(fileName) && Path.GetFileNameWithoutExtension(fileName).Length > 5;
        }
        public interface IExtensionManager //Definicja nowego interfejsu
        {
            bool IsValid(string fileName);
        }
        class FileExtensionManager : IExtensionManager
        {
            public bool IsValid(string fileName)
            {
                return true;
            }
        }
        public class ExtensionManagerFactory
        {
            private IExtensionManager customManager = null;
            public IExtensionManager Create()
            {
                if (customManager != null)
                    return customManager;
                return new FileExtensionManager();
            }
            public void SetManager(IExtensionManager mgr)
            {
                customManager = mgr;
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
