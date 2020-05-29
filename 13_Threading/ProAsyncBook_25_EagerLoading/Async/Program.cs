using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Async
{
    //Listing 5-1. Eager Loading CsvRepository
    //Eager Loading helps you to load all your needed entities at once.
    //This could affect startup time, and even result in a greater memory footprint than is ultimately required.An alternative approach
    //would be to load each CSV when the Map method is called for the first time for each CSV file; this is known as Lazy Loading.

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    public class CsvRepository
    {
        private readonly string directory;
        private Dictionary<string, List<string[]>> csvFiles;
        public CsvRepository(string directory)
        {
            this.directory = directory;
            csvFiles = new DirectoryInfo(directory)
            .GetFiles("*.csv")
            .ToDictionary(f => f.Name, f => LoadData(f.FullName).ToList());
        }


        public IEnumerable<string> Files { get { return csvFiles.Keys; } }

        //The Map method is supplied a function that can turn a CSV row (string[]) into the supplied generic type argument.
        public IEnumerable<T> Map<T>(string dataFile, Func<string[], T> map)
        {
            return csvFiles[dataFile].Skip(1).Select(map);
        }
        private IEnumerable<string[]> LoadData(string filename) //wykorzystane w LINQ
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                { yield return reader.ReadLine().Split(','); }
            }
        }
    }
}
