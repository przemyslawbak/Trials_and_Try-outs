using System;

namespace Sample
{
    internal class DataResultModel
    {
        public Guid DataGuid { get; set; }
        public string IndexName { get; set; }
        public string DataType { get; set; }
        public double ResultValue { get; set; }
        public DateTime UtcTimeStamp { get; set; }
        public bool Release { get; set; }
    }
}
