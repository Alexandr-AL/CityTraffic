using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.StoppointTimetable
{
    public class StoppointTimetable
    {
        [JsonPropertyName("stoppointId")]
        public int StoppointId { get; set; }

        [JsonPropertyName("scheduledTime")]
        public string ScheduledTime { get; set; }

        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("routeName")]
        public string RouteName { get; set; }

        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        [JsonPropertyName("routeTypeName")]
        public string RouteTypeName { get; set; }

        [JsonPropertyName("ngr")]
        public string Ngr { get; set; }

        [JsonPropertyName("lowFloor")]
        public bool LowFloor { get; set; }
    }
}
