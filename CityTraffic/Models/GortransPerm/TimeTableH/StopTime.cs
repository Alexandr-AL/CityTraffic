using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.TimeTableH
{
    public class StopTime
    {
        [JsonPropertyName("scheduledTime")]
        public string ScheduledTime { get; set; }

        [JsonPropertyName("ngr")]
        public string Ngr { get; set; }
    }

}
