using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Model saved in DB
    /// </summary>
    public class RequestJSONModel
    {
        [Key]
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
