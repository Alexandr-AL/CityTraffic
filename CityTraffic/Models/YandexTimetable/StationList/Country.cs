using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class Country
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("regions")]
        public List<Region> Regions { get; set; }

        [JsonIgnore]
        public int CodesId { get; set; }
    }
}