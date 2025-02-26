using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.News
{
    public class NewsLink
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonPropertyName("itemNumber")]
        public int ItemNumber { get; set; }
    }
}
