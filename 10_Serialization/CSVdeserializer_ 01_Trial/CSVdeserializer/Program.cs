using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;

namespace CSVdeserializer
{
    public class MyObject
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            List<MyObject> list = ReadCsv();
            foreach (var item in list)
            {
                Console.WriteLine(item.firstName + ", " + item.lastName + ", " + item.email);
            }
            Console.ReadKey();
        }

        public static List<MyObject> ReadCsv()
        {
            //holds the property mappings
            Dictionary<string, int> myObjectMap = new Dictionary<string, int>();

            List<MyObject> myObjectList = new List<MyObject>();

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(new StreamReader("data.csv"), true, '|'))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();

                for (int i = 0; i < fieldCount; i++)
                {
                    myObjectMap[headers[i]] = i; // track the index of each column name
                }

                while (csv.ReadNextRecord())
                {
                    MyObject myObj = new MyObject
                    {
                        firstName = csv[myObjectMap["firstName"]],
                        lastName = csv[myObjectMap["lastName"]],
                        email = csv[myObjectMap["email"]]
                    };

                    myObjectList.Add(myObj);
                }
            }
            return myObjectList;
        }
    }
}
