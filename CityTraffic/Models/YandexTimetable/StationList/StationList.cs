using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class StationList
    {
        [JsonPropertyName("countries")]
        public List<Country> Countries { get; set; }
    }
}