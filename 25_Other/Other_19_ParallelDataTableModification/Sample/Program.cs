using System.Data;

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
            var utcTimeStamps = Enumerable.Range(0, 50000)
              .Select(offset => start.AddMinutes(offset))
              .ToList();

            var col1Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_1",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = r.Next(0, 100),
                    UtcTimeStamp = x,
                });

            var col2Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_2",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = r.Next(0, 100000),
                    UtcTimeStamp = x,
                });

            var col3Data = utcTimeStamps
                .Select(x => new DataResultEntity()
                {
                    DataGuid = Guid.NewGuid(),
                    DataType = "COL_3",
                    IndexName = "SOME INDEX",
                    Release = true,
                    ResultValue = r.Next(0, 100000),
                    UtcTimeStamp = x,
                });

            dataFromApi.AddRange(col1Data);
            dataFromApi.AddRange(col2Data);
            dataFromApi.AddRange(col3Data);

            //create params
            bool trendsToo = false;
            int[] _intervals = new int[] { 5, 10 };
            List<string> selectedDataTypes = new List<string>() { "COL_1", "COL_2", "COL_3" };
            string mainDataType = "COL_1";

            CreateDataTableWithTrends(dataFromApi, _intervals, selectedDataTypes, mainDataType, trendsToo);
            Console.ReadKey();
        }

        public static void CreateDataTableWithTrends(List<DataResultEntity> dataFromApi, int[] _intervals, List<string> selectedDataTypes, string mainDataType, bool trendsToo)
        {
            long elapsedMsNew = 0;
            long elapsedMsOld = 0;

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

                    Parallel.ForEach(timestamps, timestamp =>
                    {
                        progress++;
                        Console.Write("\r{0}%", percentage);
                        lock (dt.Rows.SyncRoot)
                        {
                            DataRow dr = dt.NewRow();
                            dr["UtcTimeStamp"] = timestamp;
                            dt.Rows.Add(dr);
                            foreach (var dataType in selectedDataTypes)
                            {
                                dr[dataType] = dataFromApi.Where(y => y.DataType == dataType && y.UtcTimeStamp == timestamp).Select(y => y.ResultValue).First();
                            }
                        }
                        percentage = Math.Round((double)progress / timestamps.Count() * 100, 2);
                    });

                    dt.DefaultView.Sort = "UtcTimeStamp DESC";

                    dt.AcceptChanges();

                    watchOld.Stop();
                    elapsedMsOld = watchOld.ElapsedMilliseconds;

                    ExportDataTableToFile(dt, "|", true, "_exportedDataTableOld.txt");
                }

                dt.DefaultView.Sort = "UtcTimeStamp DESC";
                Console.WriteLine("Old way time (ms): " + elapsedMsOld);
            }

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

                    var percentage = 0.00;
                    var progress = 0;

                    var watchNew = System.Diagnostics.Stopwatch.StartNew();

                    Parallel.ForEach(dt.AsEnumerable(), dr =>
                    {
                        progress++;
                        Console.Write("\r{0}%", percentage);
                        dr.BeginEdit();
                        var timestamp = (DateTime)dr["UtcTimeStamp"];
                        foreach (var dataType in selectedDataTypes)
                        {
                            var xxx = dataFromApi.Where(y => y.DataType == dataType && y.UtcTimeStamp == timestamp).Select(y => y.ResultValue).First();
                            dr[dataType] = xxx;
                        }
                        dr.EndEdit();
                        percentage = Math.Round((double)progress / timestamps.Count() * 100, 2);
                    });

                    dt.DefaultView.Sort = "UtcTimeStamp DESC";

                    dt.AcceptChanges();

                    watchNew.Stop();
                    elapsedMsNew = watchNew.ElapsedMilliseconds;

                    ExportDataTableToFile(dt, "|", true, "_exportedDataTableNew.txt");
                }

                dt.DefaultView.Sort = "UtcTimeStamp DESC";

                Console.WriteLine("New way time (ms): " + elapsedMsNew);
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
        }
    }
}