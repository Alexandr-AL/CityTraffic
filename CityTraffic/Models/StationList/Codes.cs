using System.Text.Json.Serialization;

namespace CityTraffic.Models.StationList
{
    public class Codes
    {
        [JsonPropertyName("esr_code")]
        public string ESRCode { get; set; }

        [JsonPropertyName("yandex_code")]
        public string YandexCode { get; set; }
    }
}