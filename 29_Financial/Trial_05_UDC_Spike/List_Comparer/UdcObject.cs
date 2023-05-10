using Newtonsoft.Json;

namespace List_Comparer
{
    internal class UdcObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "strongBuy")]
        public int StrongBuy { get; set; }

        [JsonProperty(PropertyName = "buy")]
        public int Buy { get; set; }

        [JsonProperty(PropertyName = "hold")]
        public int Hold { get; set; }

        [JsonProperty(PropertyName = "sell")]
        public int Sell { get; set; }

        [JsonProperty(PropertyName = "strongSell")]
        public int StrongSell { get; set; }
        public decimal Multiplier { get; set; }
    }
}
