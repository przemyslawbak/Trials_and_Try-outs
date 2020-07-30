using Newtonsoft.Json;

namespace WebApp.Data
{
    public class CountryDTO
    {
        public CountryDTO() { }
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("iso2")]
        public string ISO2 { get; set; }
        [JsonProperty("iso3")]
        public string ISO3 { get; set; }
        public int TotCities { get; set; }
        #endregion
    }
}
