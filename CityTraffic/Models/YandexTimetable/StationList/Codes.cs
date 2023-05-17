using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class Codes
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("esr_code")]
        public string ESRCode { get; set; }

        [JsonPropertyName("yandex_code")]
        public string YandexCode { get; set; }
    }
}