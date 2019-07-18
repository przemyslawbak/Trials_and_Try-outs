using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Model serialized and saved in App_Data/xml/
    /// </summary>
    [XmlRoot(ElementName = "request")]
    public class RequestXMLModel
    {
        [Key]
        [JsonProperty("ix")]
        [XmlElement("ix")]
        public int Index { get; set; }

        [XmlElement("content")]
        public ContentXMLModel Content { get; set; }

    }
    [XmlRoot(ElementName = "content")]
    public class ContentXMLModel
    {
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("visits", IsNullable = true)]
        public int? Visits { get; set; }
        public bool ShouldSerializeVisits() { return Visits != null; }
        [JsonProperty("date")]
        private DateTime Date { get; set; }

        [XmlElement("date")]
        public string dateRequested
        {
            get { return Date.ToString("yyyy-MM-dd"); }
            set { Date = DateTime.ParseExact(value, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture); }
        }
    }
}
