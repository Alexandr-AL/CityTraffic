using System.Text.Json.Serialization;

namespace CityTraffic.Models.StationList
{
    public class StationList
    {
        [JsonPropertyName("countries")]
        public List<Countries> Countries { get; set; }
    }
}