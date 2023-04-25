using System.Text.Json.Serialization;
using CityTraffic.Converters.Json;

namespace CityTraffic.Models.StationList
{
    public class Stations
    {
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
    }
}