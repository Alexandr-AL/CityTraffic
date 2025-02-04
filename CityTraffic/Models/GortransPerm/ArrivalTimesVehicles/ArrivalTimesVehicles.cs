using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.ArrivalTimesVehicles
{
    public class ArrivalTimesVehicles
    {
        [JsonPropertyName("routeTypes")]
        public List<CityTraffic.Models.GortransPerm.ArrivalTimesVehicles.RouteType> RouteTypes { get; set; }
    }
}
