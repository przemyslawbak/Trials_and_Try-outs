﻿using Newtonsoft.Json;

namespace List_Comparer
{
    public class SocialObject
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "stocktwitsPosts")]
        public int Posts { get; set; }

        [JsonProperty(PropertyName = "stocktwitsComments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "stocktwitsLikes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "stocktwitsImpressions")]
        public int Impressions { get; set; }

        [JsonProperty(PropertyName = "stocktwitsSentiment")]
        public decimal Sentiment { get; set; }
        public decimal Multiplier { get; set; }
    }
}
