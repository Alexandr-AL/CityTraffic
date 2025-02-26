using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.MovingAutos
{
    public class MovingAutos
    {
        [JsonPropertyName("autos")]
        public List<Auto> Autos { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
