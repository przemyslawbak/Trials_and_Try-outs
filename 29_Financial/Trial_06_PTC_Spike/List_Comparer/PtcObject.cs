using Newtonsoft.Json;

namespace List_Comparer
{
    public class PtcObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "targetConsensus")]
        public decimal TargetConsensus { get; set; }
        public decimal Price { get; set; }
        public decimal Multiplier { get; set; }
    }
}
