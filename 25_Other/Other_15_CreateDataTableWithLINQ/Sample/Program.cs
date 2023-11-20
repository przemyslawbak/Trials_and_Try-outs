using Newtonsoft.Json;
using Sample;
using System.Data;

namespace Activator
{
    class Program
    {
        private static readonly UrlLocker _urlLocker = new UrlLocker();
        private static HttpClient _httpClient = new HttpClient();
        private static int[] _intervals = new int[] { 5, 10, 15 };
        static void Main(string[] args)
        {
            RunProgram().Wait();
            Console.WriteLine("finito");
            Console.ReadKey();
        }

        private static async Task RunProgram()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_urlLocker.GetUrl());
            string jsonString = await GetJsonFromResponse(response);
            var dataFromApi = GetSignalsObjectFromJson(jsonString);
            dataFromApi = RemoveDuplicatedTimeStamps(dataFromApi);
            dataFromApi = ProcessMissingDataFromApi(dataFromApi);
            var xxx = ProcessMissingDataFromApiUPDATED(dataFromApi);
            DataTable dataTable = await Task.Run(() => CreateDataTableWithTrends(dataFromApi, _intervals));
        }

        private static DataTable CreateDataTableWithTrends(List<DataResultModel> dataFromApi, int[] intervals) //TODO: optimization
        {
            var dt = new DataTable();

            if (dataFromApi != null)
            {

                var dataTypes = dataFromApi.GroupBy(x => x.DataType).Select(x => x.Key).ToList();
                var timestamps = dataFromApi.Where(x => x.DataType == "Index_SPXINDEX").Select(x => x.UtcTimeStamp).ToList();

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

                    dt.Rows.Add(dr);

                    foreach (var dataType in dataTypes)
                    {
                        dr[dataType] = dataFromApi.Where(x => x.DataType == dataType && x.UtcTimeStamp == timeStamp).Select(x => x.ResultValue).First();
                    }
                }

                //compute and add means and trends
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
            }

            return dt;
        }

        private static List<DataResultModel> ProcessMissingDataFromApi(List<DataResultModel> dataList)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var dataTypes = dataList.GroupBy(x => x.DataType).Select(x => x.Key).ToList();
            var timeStampsSpx = dataList.Where(x => x.DataType == "Index_SPXINDEX").Select(x => x.UtcTimeStamp).ToList();

            foreach (var dataTypeName in dataTypes)
            {
                var dataForDataType = dataList
                    .Where(x => x.DataType == dataTypeName);
                var dataTypeTimeStamps = dataForDataType
                    .Select(x => x.UtcTimeStamp);
                var minutesNotCoveredInDataType = timeStampsSpx
                    .Where(x => !dataTypeTimeStamps.Contains(x))
                    .OrderBy(x => x);

                foreach (var missingimeStamp in minutesNotCoveredInDataType)
                {
                    var nearestDiff = dataTypeTimeStamps
                        .Min(date => Math.Abs((date - missingimeStamp).Ticks));
                    var nearestDateTime = dataTypeTimeStamps
                        .Where(date => Math.Abs((date - missingimeStamp).Ticks) == nearestDiff)
                        .First();
                    var nearestValue = dataForDataType
                        .Where(x => x.UtcTimeStamp == nearestDateTime)
                        .Select(x => x.ResultValue)
                        .First();
                    var itemToBeAdded = new DataResultModel()
                    {
                        DataGuid = new Guid(),
                        ResultValue = nearestValue,
                        DataType = dataTypeName,
                        UtcTimeStamp = missingimeStamp,
                        IndexName = dataList
                        .Select(x => x.IndexName)
                        .First(),
                        Release = false,
                    };
                    dataList.Add(itemToBeAdded);
                }
            }

            var toReturn = new List<DataResultModel>();

            foreach (var dataTypeName in dataTypes)
            {
                var toAdd = dataList.Where(x => x.DataType == dataTypeName).Where(x => timeStampsSpx.Contains(x.UtcTimeStamp)).ToList();
                toReturn.AddRange(toAdd);
            }

            toReturn = toReturn.OrderBy(x => x.UtcTimeStamp).Distinct().ToList(); //ascending

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("ORYGINAL: (ms) " + elapsedMs);

            return toReturn;
        }
        
        private static List<DataResultModel> ProcessMissingDataFromApiUPDATED(List<DataResultModel> dataList) //TODO: optimization
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var dataTypes = dataList.GroupBy(x => x.DataType).Select(x => x.Key).ToList();
            var timeStampsSpx = dataList.Where(x => x.DataType == "Index_SPXINDEX").Select(x => x.UtcTimeStamp).ToList();

            foreach (var dataTypeName in dataTypes)
            {

                var dataTypeTimeStamps = dataList
                    .Where(x => x.DataType == dataTypeName)
                    .Select(x => x.UtcTimeStamp);
                var minutesNotCoveredInDataType = timeStampsSpx
                    .Where(x => !dataTypeTimeStamps.Contains(x))
                    .OrderBy(x => x);

                var ranged = minutesNotCoveredInDataType.Select(x => new DataResultModel()
                {
                    UtcTimeStamp = x,
                    DataGuid = new Guid(),
                    DataType = dataTypeName,
                    IndexName = dataList
                    .Select(y => y.IndexName)
                    .First(),
                    Release = false,
                    ResultValue = dataList
                    .Where(y => y.DataType == dataTypeName)
                    .Where(y => y.UtcTimeStamp < x)
                    .Select(z => z.ResultValue).Last()
                });

                dataList.AddRange(ranged);
            }

            var toReturn = new List<DataResultModel>();

            foreach (var dataTypeName in dataTypes)
            {
                var toAdd = dataList.Where(x => x.DataType == dataTypeName).Where(x => timeStampsSpx.Contains(x.UtcTimeStamp)).ToList();
                toReturn.AddRange(toAdd);
            }

            toReturn = toReturn.OrderBy(x => x.UtcTimeStamp).Distinct().ToList(); //ascending

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("UPDATED: (ms) " + elapsedMs);

            return toReturn;
        }

        private static List<DataResultModel> RemoveDuplicatedTimeStamps(List<DataResultModel> dataFromApi)
        {
            List<DataResultModel> toReturn = new List<DataResultModel>();

            var dataTypes = dataFromApi.GroupBy(x => x.DataType).Select(x => x.Key).ToList();

            foreach (var name in dataTypes)
            {
                var toAdd = dataFromApi
                    .Where(x => x.DataType == name)
                    .GroupBy(x => x.UtcTimeStamp)
                    .Select(x => x.First());

                toReturn.AddRange(toAdd);
            }

            return toReturn;
        }

        private static List<DataResultModel> GetSignalsObjectFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<DataResultModel>>(jsonString);
        }

        private static Task<string> GetJsonFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync();
            }

            return Task.Run(() => string.Empty);
        }


    }
}



