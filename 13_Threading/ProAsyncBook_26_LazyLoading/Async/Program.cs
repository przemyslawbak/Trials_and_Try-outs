using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Async
{
    //Listing 5-3 to 7. Thread-Safe Lazy Loading. Less Possibility for Contention

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    public class CsvRepository
    {
        public class VirtualCsv
        {
            public List<string[]> Value;
        }
        private readonly string directory;
        private Dictionary<string, VirtualCsv> csvFiles;
        public CsvRepository(string directory)
        {
            this.directory = directory;
            csvFiles = new DirectoryInfo(directory)
            .GetFiles("*.csv")
            .ToDictionary<FileInfo, string, VirtualCsv>(f => f.Name, f => new VirtualCsv());
        }
        public IEnumerable<string> Files { get { return csvFiles.Keys; } }
        public IEnumerable<T> Map<T>(string dataFile, Func<string[], T> map)
        {
            return LazyLoadData(dataFile).Skip(1).Select(map);
        }
        private IEnumerable<string[]> LoadData(string filename) //wykorzystane w LazyLoadData
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                { yield return reader.ReadLine().Split(','); }
            }
        }

        private IEnumerable<string[]> LazyLoadData(string filename)
        {
            List<string[]> csvFile = csvFiles[filename].Value;
            if (csvFile == null)
            {
                lock (csvFiles[filename])
                {
                    csvFile = csvFiles[filename].Value;
                    if (csvFile == null)
                    {
                        csvFile = LoadData(Path.Combine(directory, filename)).ToList();
                        csvFiles[filename].Value = csvFile;
                    }
                }
            }
            return csvFile;
        }
    }
}
