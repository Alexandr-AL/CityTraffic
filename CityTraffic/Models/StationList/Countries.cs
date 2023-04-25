using System.Text.Json.Serialization;

namespace CityTraffic.Models.StationList
{
    public class Countries
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("regions")]
        public List<Regions> Regions { get; set; }
    }
}