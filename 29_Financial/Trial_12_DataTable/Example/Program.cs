using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFeed = new List<DataModel>()
            {
                new DataModel() { ResultValue = 21.12M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 25, 1) },
                new DataModel() { ResultValue = 21.22M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 26, 1) },
                new DataModel() { ResultValue = 21.32M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 27, 1) },
                new DataModel() { ResultValue = 21.42M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 28, 1) },
                new DataModel() { ResultValue = 21.52M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
                new DataModel() { ResultValue = 1.12M, DataType = "Index_1", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 25, 1) },
                new DataModel() { ResultValue = 1.22M, DataType = "Index_1", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 26, 1) },
                new DataModel() { ResultValue = 1.32M, DataType = "Index_1", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 27, 1) },
                new DataModel() { ResultValue = 1.42M, DataType = "Index_1", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 28, 1) },
                new DataModel() { ResultValue = 1.52M, DataType = "Index_1", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
                new DataModel() { ResultValue = 5.12M, DataType = "Index_2", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 25, 1) },
                new DataModel() { ResultValue = 6.22M, DataType = "Index_2", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 26, 1) },
                new DataModel() { ResultValue = 7.32M, DataType = "Index_2", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 27, 1) },
                new DataModel() { ResultValue = 6.42M, DataType = "Index_2", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 28, 1) },
                new DataModel() { ResultValue = 8.52M, DataType = "Index_2", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
                new DataModel() { ResultValue = 823.12M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 25, 1) },
                new DataModel() { ResultValue = 856.22M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 26, 1) },
                new DataModel() { ResultValue = 843.32M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 27, 1) },
                new DataModel() { ResultValue = 856.42M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 28, 1) },
                new DataModel() { ResultValue = 871.52M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
            };

            var dataTypes = dataFeed.GroupBy(x => x.DataType).Select(x => x.Key).ToList();
            var timestamps = dataFeed.Where(x => x.DataType == "Index_Base").Select(x => x.UtcTimeStamp).ToList();
            var dt = new DataTable();

            //add columns
            dt.Columns.Add("UtcTimeStamp", typeof(DateTime));
            foreach (var dataType in dataTypes)
            {
                dt.Columns.Add(dataType, typeof(decimal));
            }

            foreach (var timeStamp in timestamps)
            {
                DataRow dr = dt.NewRow();
                dr["UtcTimeStamp"] = timeStamp;

                foreach (var dataType in dataTypes)
                {
                    dr[dataType] = dataFeed.Where(x => x.DataType == dataType && x.UtcTimeStamp == timeStamp).Select(x => x.ResultValue).First();
                }

                dt.Rows.Add(dr);
            }
        }
    }
}
