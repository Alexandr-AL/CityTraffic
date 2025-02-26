using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.StoppointRoutes
{
    public class StoppointRoutes
    {
        [JsonPropertyName("routeTypes")]
        public List<RouteType> RouteTypes { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
