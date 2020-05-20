using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing ?-?. Task Body with a Parameter
    class Program
    {
        private static void Main(string[] args)
        {
            var importer = new DataImporter(@"C:\data");
            Task.Factory.StartNew(importer.Import).Wait();
        }
    }

    public class DataImporter
    {
        private readonly string _directory;
        public DataImporter(string directory)
        {
            _directory = directory;
        }
        public void Import()
        {
            string directory = _directory;
            Console.WriteLine(directory);
        }
    }
}
