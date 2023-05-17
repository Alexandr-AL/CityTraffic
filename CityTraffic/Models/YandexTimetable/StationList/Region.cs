using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class Region
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("settlements")]
        public List<Settlement> Settlements { get; set; }

        [JsonIgnore]
        public int CodesId { get; set; }

        [JsonIgnore]
        public int CountryId { get; set; }

        [JsonIgnore]
        public Country Country { get; set; }
    }
}