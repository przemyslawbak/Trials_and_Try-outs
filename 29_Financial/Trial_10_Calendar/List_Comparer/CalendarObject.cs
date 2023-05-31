using Newtonsoft.Json;

namespace List_Comparer
{
    public class CalendarObject
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
    }
}
