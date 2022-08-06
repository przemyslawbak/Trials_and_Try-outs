using System;
using System.IO;

namespace LogAn
{
    public interface ILogger
    {
        void LogError(string message);
    }
    public class LogAnalyzer
    {
        private ILogger logger;
        public LogAnalyzer(ILogger service)
        {
            this.logger = service;
        }
        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                //Rejestrowanie błędów
                //w kodzie produkcyjnym
                logger.LogError("Nazwa pliku jest zbyt krótka:" + fileName);
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
