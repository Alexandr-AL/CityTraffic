using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.Search
{
    public class Stoppoint
    {
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("course")]
        public int Course { get; set; }

        [JsonPropertyName("labelXOffset")]
        public object LabelXOffset { get; set; }

        [JsonPropertyName("labelYOffset")]
        public object LabelYOffset { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("routes")]
        public string Routes { get; set; }

        [JsonPropertyName("routeTypes")]
        public List<string> RouteTypes { get; set; }
    }
}
