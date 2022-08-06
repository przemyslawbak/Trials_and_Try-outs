using System;
using System.IO;

namespace LogAn
{
    public interface IWebService
    {
        void LogError(string message);
    }
    public class LogAnalyzer
    {
        private IWebService service;
        public LogAnalyzer(IWebService service)
        {
            this.service = service;
        }
        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                //Rejestrowanie błędów
                //w kodzie produkcyjnym
                service.LogError("Nazwa pliku jest zbyt krótka:" + fileName);
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
