using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.TimeTableH
{
    public class Route
    {
        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("routeName")]
        public string RouteName { get; set; }

        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }
    }

}
