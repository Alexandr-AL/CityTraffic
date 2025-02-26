using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.TimeTableH
{
    public class TimeTable
    {
        [JsonPropertyName("hour")]
        public int Hour { get; set; }

        [JsonPropertyName("stopTimes")]
        public List<StopTime> StopTimes { get; set; }
    }

}
