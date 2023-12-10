using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace List_Comparer
{
    public class CalendarObjectHistory
    {
        [JsonProperty(PropertyName = "country")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "previous")]
        public decimal? Previous { get; set; }

        [JsonProperty(PropertyName = "estimate")]
        public decimal? Estimate { get; set; }

        [JsonProperty(PropertyName = "actual")]
        public decimal? Actual { get; set; }

        [JsonProperty(PropertyName = "impact")]
        public string Impact { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime EstTimeStamp { get; set; }

        public List<CalendarObjectHistory> DataSet { get; set; }
        public decimal ResultValue { get; set; }
        public DateTime UtcTimeStamp { get; set; }
    }
}
