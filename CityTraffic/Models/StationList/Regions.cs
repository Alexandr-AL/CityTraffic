using System.Text.Json.Serialization;

namespace CityTraffic.Models.StationList
{
    public class Regions
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("settlements")]
        public List<Settlements> Settlements { get; set; }
    }
}