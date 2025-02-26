using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.Board
{
    public class Board
    {
        [JsonPropertyName("boardId")]
        public int BoardId { get; set; }

        [JsonPropertyName("stopName")]
        public string StopName { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }
}
