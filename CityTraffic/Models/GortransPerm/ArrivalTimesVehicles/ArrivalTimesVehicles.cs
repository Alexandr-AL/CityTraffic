using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.ArrivalTimesVehicles
{
    public class ArrivalTimesVehicles
    {
        [JsonPropertyName("routeTypes")]
        public List<RouteType> RouteTypes { get; set; }
    }
}
