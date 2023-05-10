using Newtonsoft.Json;

namespace List_Comparer
{
    internal class DcfObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "dcf")]
        public decimal Dcf { get; set; }

        [JsonProperty(PropertyName = "Stock Price")]
        public decimal StockPrice { get; set; }
        public decimal Multiplier { get; set; }
    }
}
