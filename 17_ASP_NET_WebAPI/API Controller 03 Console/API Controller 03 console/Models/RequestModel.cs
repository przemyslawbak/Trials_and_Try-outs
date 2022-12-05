using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Controller_03_console.Models
{
    //model
    public class RequestModel
    {
        [JsonProperty("ix")]
        public int Index { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("visits")]
        public int? Visits { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
