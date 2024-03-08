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


        }
    }
}