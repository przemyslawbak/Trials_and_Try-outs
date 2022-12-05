using System;
using System.Collections.Concurrent;
using System.IO;

namespace _32_Zwalnianie_zasobów
{
    //Wywołanie metody Dispose() z poziomu finalizatora
    class Test : IDisposable
    {
        public void Dispose() // NIE jest wirtualna
        {
            Dispose(true);
            GC.SuppressFinalize(this); // uniemożliwia wywołanie finalizatora
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // wywołanie metody Dispose() w innych obiektach, których właścicielem jest ten egzemplarz
                // w tym miejscu można odwołać się do innych obiektów zawierających finalizatory
                // ...
            }
            // zwolnienie niezarządzanych zasobów, których właścicielem jest (jedynie) ten obiekt
            // ...
        }
        ~Test()
        {
            Dispose(false);
        }
    }

    //wskrzeszenie
    public class TempFileRef
    {
        static ConcurrentQueue<TempFileRef> _failedDeletions
        = new ConcurrentQueue<TempFileRef>();
        public readonly string FilePath;
        public Exception DeletionError { get; private set; }
        public TempFileRef(string filePath) { FilePath = filePath; }
        ~TempFileRef()
        {
            try { File.Delete(FilePath); }
            catch (Exception ex)
            {
                DeletionError = ex;
                _failedDeletions.Enqueue(this); // wskrzeszenie
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
