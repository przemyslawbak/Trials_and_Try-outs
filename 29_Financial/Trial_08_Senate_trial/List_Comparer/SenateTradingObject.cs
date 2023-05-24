using Newtonsoft.Json;
using System;

namespace List_Comparer
{
    public class SenateTradingObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "transactionDate")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }
        public decimal Multiplier { get; set; }
    }
}
