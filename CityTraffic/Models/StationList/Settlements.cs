using System.Text.Json.Serialization;

namespace CityTraffic.Models.StationList
{
    public class Settlements
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("stations")]
        public List<Stations> Stations { get; set; }
    }
}