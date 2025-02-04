using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.StoppointRoutes
{
    public class RouteType
    {
        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }
    }
}
