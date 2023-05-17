using CityTraffic.Converters.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.YandexTimetable.StationList
{
    public class Station
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("codes")]
        public Codes Codes { get; set; }

        [JsonPropertyName("station_type")]
        public string StationType { get; set; }

        [JsonPropertyName("transport_type")]
        public string TransportType { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        [JsonPropertyName("latitude")]
        [JsonConverter(typeof(DoubleNullConverter))]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [JsonConverter(typeof(DoubleNullConverter))]
        public double Longitude { get; set; }

        [JsonIgnore]
        public int CodesId { get; set; }

        [JsonIgnore]
        public int SettlementId { get; set; }

        [JsonIgnore]
        public Settlement Settlement { get; set; }
    }
}