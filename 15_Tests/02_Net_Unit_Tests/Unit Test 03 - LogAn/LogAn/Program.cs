using System;

namespace LogAn
{
    public class LogAnalyzer
    {
        public bool WasLastFileNameValid { get; set; }
        public bool IsValidLogFileNameee(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("należy podać nazwę pliku");
            }
            if (!fileName.EndsWith(".SLF", StringComparison.CurrentCultureIgnoreCase))
            {
                WasLastFileNameValid = false; //zmiana stanu systemu
                return false;
            }
            WasLastFileNameValid = true; //zmiana stanu systemu
            return true;
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
