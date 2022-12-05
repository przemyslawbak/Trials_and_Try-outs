using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRESTLib.Models
{
    public class Order
    {
        public string ObjectId { get; set; }
        public string OrderNumber { get; set; }
        [JsonIgnore]
        public List<Item> Items { get; set; }
    }
}
