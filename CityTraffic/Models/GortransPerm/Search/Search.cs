using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.Search
{
    public class Search
    {
        [JsonPropertyName("searchResults")]
        public List<SearchResult> SearchResults { get; set; }
    }
}
