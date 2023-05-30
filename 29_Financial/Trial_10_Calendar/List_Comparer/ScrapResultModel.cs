using System;

namespace List_Comparer
{
    internal class ScrapResultModel
    {
        public bool Success { get; set; } = false;
        public object ResultValue { get; set; } = new object();
        public DateTime UtcTimeStamp { get; set; } = DateTime.UtcNow;
        public double SpeedSeconds { get; set; } = 10000;
        public string Host { get; set; } = "initial";
        public string Error { get; set; } = "initial";
        public string Method { get; set; } = "initial";
        public string DataType { get; set; } = "initial";
        public string IndexName { get; set; } = "initial";
        public int FailedTimesInRow { get; set; }
        public Interval Interval { get; set; }
    }
}
