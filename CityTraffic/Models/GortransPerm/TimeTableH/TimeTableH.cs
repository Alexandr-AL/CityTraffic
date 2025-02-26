using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.TimeTableH
{
    public class TimeTableH
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("route")]
        public Route Route { get; set; }

        [JsonPropertyName("timeTable")]
        public List<TimeTable> TimeTable { get; set; }

        [JsonPropertyName("footnotes")]
        public List<object> Footnotes { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

}
