using Newtonsoft.Json;

namespace List_Comparer
{
    internal class PriceObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
    }
}
