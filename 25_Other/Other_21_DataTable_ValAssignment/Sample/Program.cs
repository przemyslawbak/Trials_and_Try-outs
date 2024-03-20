using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //create base
            var dataFromApi = new List<DataResultEntity>();
            Random r = new Random();
            var start = DateTime.Now.AddYears(-1);
            var end = DateTime.Now;

            //create list
            var utcTimeStamps = Enumerable.Range(0, 1000)
              .Select(offset => start.AddMinutes(offset))
              .ToList();

            var col1Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_1",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = (decimal)r.Next(0, 100) / 100,
                    UtcTimeStamp = x,
                }).ToList();

            var col4Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_4",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = (decimal)r.Next(0, 100) / 100,
                    UtcTimeStamp = x,
                }).ToList();

            var col2Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_2",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = r.Next(0, 100000),
                    UtcTimeStamp = x,
                }).ToList();

            var col3Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_3",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = r.Next(0, 100000),
                    UtcTimeStamp = x,
                }).ToList();

            dataFromApi.AddRange(col1Data);
            dataFromApi.AddRange(col2Data);
            dataFromApi.AddRange(col3Data);
            dataFromApi.AddRange(col4Data);

            System.IO.File.WriteAllLines("col1Data.txt", col1Data.Select(x => x.UtcTimeStamp + "|" + x.ResultValue.ToString()));
            System.IO.File.WriteAllLines("col2Data.txt", col2Data.Select(x => x.UtcTimeStamp + "|" + x.ResultValue.ToString()));
            System.IO.File.WriteAllLines("col3Data.txt", col3Data.Select(x => x.UtcTimeStamp + "|" + x.ResultValue.ToString()));
            System.IO.File.WriteAllLines("col4Data.txt", col4Data.Select(x => x.UtcTimeStamp + "|" + x.ResultValue.ToString()));


            //create params
            bool trendsToo = false;
            int[] _intervals = new int[] { 5, 10 };
            List<string> selectedDataTypes = new List<string>() { "COL_1", "COL_2", "COL_3", "COL_4" };
            string mainDataType = "COL_1";

            var dt = CreateDataTableWithTrends(dataFromApi, _intervals, selectedDataTypes, mainDataType, trendsToo);

            Console.WriteLine("Computing diff...");
            int[] intervals = new int[] { 15, 60, 90 };
            dt = AddDiff(dt, intervals);

            ExportDataTableToFile(dt, "|", true, "_exportedDataTable.txt");

            Console.ReadKey();
        }

        public static DataTable AddDiff(DataTable dt, int[] intervals)
        {
            dt.Columns.Add("Diff", typeof(decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    var row = dt.Rows[i];
                    var SPXFUTURES = row.Field<decimal?>("COL_1");
                    var SPXINDEX = row.Field<decimal?>("COL_4");

                    var futuresDiff = SPXINDEX - SPXFUTURES;
                    row["Diff"] = futuresDiff;
                }
                catch
                {
                    var row = dt.Rows[i];
                    row["Diff"] = 0M;
                }
            }

            dt.AcceptChanges();

            return dt;
        }

        public static DataTable CreateDataTableWithTrends(List<DataResultEntity> dataFromApi, int[] _intervals, List<string> selectedDataTypes, string mainDataType, bool trendsToo)
        {
            using (DataTable dt = new DataTable())
            {
                if (dataFromApi != null)
                {
                    int result = selectedDataTypes.IndexOf(mainDataType);

                    if (result == -1)
                    {
                        selectedDataTypes.Add(mainDataType);
                    }

                    var timestamps = dataFromApi.Where(x => x.DataType == mainDataType).Select(x => x.UtcTimeStamp);

                    //add columns
                    dt.Columns.Add("UtcTimeStamp", typeof(DateTime));
                    Console.WriteLine("Adding columns...");
                    foreach (var dataType in selectedDataTypes)
                    {
                        dt.Columns.Add(dataType, typeof(decimal));

                        if (trendsToo)
                        {
                            foreach (var interval in _intervals)
                            {
                                dt.Columns.Add(dataType + "_" + interval + "_mean", typeof(decimal));
                                dt.Columns.Add(dataType + "_" + interval + "_trend", typeof(decimal));
                            }
                        }
                    }
                    dt.AcceptChanges();

                    Console.WriteLine("Adding timestamps...");

                    foreach (var timestamp in timestamps)
                    {
                        DataRow dr = dt.NewRow();
                        dr["UtcTimeStamp"] = timestamp;
                        dt.Rows.Add(dr);
                    }

                    dt.AcceptChanges();

                    Console.WriteLine("Adding existing data...");
                    //add existing data
                    var watchOld = System.Diagnostics.Stopwatch.StartNew();

                    var percentage = 0.00;
                    var progress = 0;

                    Parallel.ForEach(dt.AsEnumerable(), dr =>
                    {
                        progress++;
                        Console.Write("\r{0}%", percentage);
                        var timestamp = (DateTime)dr["UtcTimeStamp"];

                        dr.BeginEdit();
                        foreach (var dataType in selectedDataTypes)
                        {
                            var xxx = dataFromApi.Where(y => y.DataType == dataType && y.UtcTimeStamp == timestamp).Select(y => y.ResultValue).ToList();
                            var res = xxx.First();

                            dr.SetField(dataType, res);
                        }
                        dr.EndEdit();

                        percentage = Math.Round((double)progress / timestamps.Count() * 100, 2);
                    });
                    dt.AcceptChanges();

                    //added
                    Console.WriteLine("filling Nulls..."); //todo: fix timestamp if null

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (var dataType in selectedDataTypes)
                        {
                            if (dt.Rows[i][dataType].ToString() == "")
                            {
                                var replacement = 0M;
                                if (i > 0)
                                {
                                    replacement = (decimal)dt.Rows[i - 1][dataType];
                                }

                                var dr = dt.Rows[i];
                                dr.SetField(dataType, replacement);
                            }
                        }
                    };

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["UtcTimeStamp"].ToString() == "")
                        {
                            var dr = dt.Rows[i];
                            dr.Delete();
                        }

                        if (i > 0 && dt.Rows[i]["UtcTimeStamp"] == dt.Rows[i - 1]["UtcTimeStamp"])
                        {
                            var dr = dt.Rows[i];
                            dr.Delete();
                        }
                    }

                    dt.AcceptChanges();

                    try
                    {
                        dt.DefaultView.Sort = "UtcTimeStamp DESC";

                        dt.AcceptChanges();
                    }
                    catch //try again
                    {
                        Console.WriteLine("Error occured, computing again...");
                        return CreateDataTableWithTrends(dataFromApi, _intervals, selectedDataTypes, mainDataType, trendsToo);
                    }
                }

                return dt;
            }
        }

        public static void ExportDataTableToFile(DataTable datatable, string delimited, bool exportcolumnsheader, string file)
        {
            StreamWriter str = new StreamWriter(file, false, System.Text.Encoding.Default);
            if (exportcolumnsheader)
            {
                string Columns = string.Empty;
                foreach (DataColumn column in datatable.Columns)
                {
                    Columns += column.ColumnName + delimited;
                }
                str.WriteLine(Columns.Remove(Columns.Length - 1, 1));
            }
            foreach (DataRow datarow in datatable.Rows)
            {
                string row = string.Empty;
                foreach (object items in datarow.ItemArray)
                {
                    row += items.ToString() + delimited;
                }
                str.WriteLine(row.Remove(row.Length - 1, 1));
            }
            str.Flush();
            str.Close();

            Console.WriteLine("Saved...");
        }
    }
}