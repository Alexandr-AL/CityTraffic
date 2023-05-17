using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class Settlement
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("stations")]
        public List<Station> Stations { get; set; }

        [JsonIgnore]
        public int CodesId { get; set; }

        [JsonIgnore]
        public int RegionId { get; set; }

        [JsonIgnore]
        public Region Region { get; set; }
    }
}