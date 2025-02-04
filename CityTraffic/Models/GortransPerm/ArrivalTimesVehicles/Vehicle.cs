using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.ArrivalTimesVehicles
{
    public class Vehicle
    {
        [JsonPropertyName("arrivalTime")]
        public string ArrivalTime { get; set; }

        [JsonPropertyName("lowFloor")]
        public bool LowFloor { get; set; }

        [JsonPropertyName("arrivalMinutes")]
        public int ArrivalMinutes { get; set; }
    }
}