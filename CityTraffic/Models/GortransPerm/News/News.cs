using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.News
{
    public class News
    {
        [JsonPropertyName("newsLinks")]
        public List<NewsLink> NewsLinks { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
