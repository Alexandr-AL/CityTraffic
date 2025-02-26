using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.Search
{
    public class SearchResult
    {
        [JsonPropertyName("resultId")]
        public string ResultId { get; set; }

        [JsonPropertyName("resultType")]
        public string ResultType { get; set; }

        [JsonPropertyName("resultTitle")]
        public string ResultTitle { get; set; }

        [JsonPropertyName("resultUrlPart")]
        public string ResultUrlPart { get; set; }

        [JsonPropertyName("stoppoint")]
        public Stoppoint Stoppoint { get; set; }
    }
}
