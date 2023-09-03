using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Example
{
    class Program
    {
        private static int[] _intervals = new int[] { 2, 3 };
        static void Main(string[] args)
        {
            var dataFeed = new List<DataModel>()
            {
                new DataModel() { ResultValue = 21.12M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 25, 1) },
                new DataModel() { ResultValue = 21.22M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 26, 1) },
                new DataModel() { ResultValue = 21.32M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 27, 1) },
                new DataModel() { ResultValue = 21.42M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 28, 1) },
                new DataModel() { ResultValue = 20.52M, DataType = "Index_Base", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
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
                new DataModel() { ResultValue = 821.52M, DataType = "Index_3", UtcTimeStamp = new DateTime(2023, 9, 3, 13, 29, 1) },
            };

            var dataTypes = dataFeed.GroupBy(x => x.DataType).Select(x => x.Key).ToList();
            var timestamps = dataFeed.Where(x => x.DataType == "Index_Base").Select(x => x.UtcTimeStamp).ToList();
            var dt = new DataTable();

            //add columns
            dt.Columns.Add("UtcTimeStamp", typeof(DateTime));
            foreach (var dataType in dataTypes)
            {
                dt.Columns.Add(dataType, typeof(decimal));

                foreach (var interval in _intervals)
                {
                    dt.Columns.Add(dataType + "_" + interval + "_mean", typeof(decimal));
                    dt.Columns.Add(dataType + "_" + interval + "_trend", typeof(decimal));
                }
            }

            //add existing data
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

            foreach (var dataType in dataTypes)
            {
                foreach (var interval in _intervals)
                {
                    var meanColName = dataType + "_" + interval + "_mean";
                    var trendColName = dataType + "_" + interval + "_trend";

                    decimal lastVal = 0;

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (i >= interval)
                        {
                            decimal sumOfLastN = 0;
                            for (int j = 0; j < interval; j++)
                            {
                                sumOfLastN = sumOfLastN + (decimal)dt.Rows[i - j][dataType];
                            }

                            dt.Rows[i][meanColName] = sumOfLastN / interval;
                            lastVal = sumOfLastN / interval;
                        }
                        else
                        {
                            dt.Rows[i][meanColName] = lastVal;
                        }
                    }

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (i >= 1)
                        {
                            var direction = (decimal)dt.Rows[i][meanColName] - (decimal)dt.Rows[i - 1][meanColName];

                            if (direction >= 0)
                            {
                                dt.Rows[i][trendColName] = 1;
                            }
                            else
                            {
                                dt.Rows[i][trendColName] = -1;
                            }
                        }
                        else
                        {
                            dt.Rows[i][trendColName] = 1;
                        }
                    }

                    var meanVals = dt.AsEnumerable().Select(x => x[meanColName]).ToList();
                    var trendVals = dt.AsEnumerable().Select(x => x[trendColName]).ToList();
                }
            }

            //todo: replace zeroes with nearest
        }
    }
}
