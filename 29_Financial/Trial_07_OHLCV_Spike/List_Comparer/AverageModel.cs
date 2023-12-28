using Newtonsoft.Json;

namespace List_Comparer
{
    public class AverageModel
    {

        [JsonProperty(PropertyName = "priceAvg50")]
        public decimal PriceAvg50 { get; set; }

        [JsonProperty(PropertyName = "priceAvg200")]
        public decimal PriceAvg200 { get; set; }
    }
}
