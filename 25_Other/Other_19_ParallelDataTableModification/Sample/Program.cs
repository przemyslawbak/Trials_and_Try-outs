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

            var dt = CreateDataTableWithTrends(dataFromApi, _intervals, selectedDataTypes, mainDataType, trendsToo);
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

                    Console.WriteLine("Adding existing data...");
                    //add existing data

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

                    dt.AcceptChanges();

                    if (trendsToo)
                    {
                        //compute and add means and trends
                        foreach (var dataType in selectedDataTypes)
                        {
                            Console.WriteLine("Computing data table for " + dataType);
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
                            }
                        }
                        dt.AcceptChanges();
                    }
                }

                dt.DefaultView.Sort = "UtcTimeStamp DESC";

                return dt;
            }
        }
    }
}