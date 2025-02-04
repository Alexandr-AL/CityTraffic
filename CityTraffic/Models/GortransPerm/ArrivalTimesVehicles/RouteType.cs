using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.ArrivalTimesVehicles
{
    public class RouteType
    {
        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        [JsonPropertyName("routeTypeName")]
        public string RouteTypeName { get; set; }

        [JsonPropertyName("routes")]
        public List<Route> Routes { get; set; }
    }
}
