using System;
using System.Collections.Generic;
using System.IO;

namespace Example
{
    class Program
    {
        private static readonly string _dataFilename = "jsw_d.csv";
        static void Main(string[] args)
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", _dataFilename);
            using (StreamReader reader = new StreamReader(dataPath))
            {
                List<string> listA = new List<string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    listA.Add(values[4]);
                }
            }
        }
    }
}
