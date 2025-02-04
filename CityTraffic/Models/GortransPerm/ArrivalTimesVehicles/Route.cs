using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.ArrivalTimesVehicles
{
    public class Route
    {
        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("vehicles")]
        public List<Vehicle> Vehicles { get; set; }
    }
}
