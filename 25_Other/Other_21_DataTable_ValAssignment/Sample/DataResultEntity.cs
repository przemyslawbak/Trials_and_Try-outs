﻿using System.ComponentModel.DataAnnotations;

namespace Sample
{
    internal class DataResultEntity
    {
        [Key]
        public Guid DataGuid { get; set; }
        public string IndexName { get; set; }
        public string DataType { get; set; }
        public decimal ResultValue { get; set; }
        public DateTime UtcTimeStamp { get; set; }
        public bool Release { get; set; }
    }
}
