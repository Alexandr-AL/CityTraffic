using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.MovingAutos
{
    public class Auto
    {
        [JsonPropertyName("kodPe")]
        public int KodPe { get; set; }

        [JsonPropertyName("routeType")]
        public int RouteType { get; set; }

        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("n")]
        public double N { get; set; }

        [JsonPropertyName("e")]
        public double E { get; set; }

        [JsonPropertyName("course")]
        public int Course { get; set; }

        [JsonPropertyName("gosNom")]
        public string GosNom { get; set; }

        [JsonPropertyName("ngr")]
        public string Ngr { get; set; }

        [JsonPropertyName("lf")]
        public bool Lf { get; set; }

        [JsonPropertyName("t")]
        public string T { get; set; }

        [JsonPropertyName("ath")]
        public string Ath { get; set; }
    }
}
