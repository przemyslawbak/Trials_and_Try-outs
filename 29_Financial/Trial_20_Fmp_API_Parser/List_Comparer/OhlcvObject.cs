using Newtonsoft.Json;
using System;

namespace List_Comparer
{
    internal class OhlcvObject
    {
        [JsonProperty(PropertyName = "date")]
        public DateTime EstTimeStamp { get; set; }

        [JsonProperty(PropertyName = "open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "high")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        public decimal Capital { get; set; }
        public string Symbol { get; set; }
        public DateTime UtcTimeStamp { get; set; }

    }
}
