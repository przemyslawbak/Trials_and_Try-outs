using Newtonsoft.Json;

namespace Sample
{
    internal class SymbolModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("exchangeShortName")]
        public string ExchangeShortName { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
