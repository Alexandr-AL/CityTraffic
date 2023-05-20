using CityTraffic.Models.Interfaces;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.Entities
{
    public class Stoppoint : IEntity, IStoppoint
    {
        public int Id { get;}

        [JsonPropertyName("stoppointId")]
        public int StoppointId { get; set; }

        [JsonPropertyName("stoppointName")]
        public string StoppointName { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        public FavoritesStoppoint FavoritesStoppoint { get; set; }

    }
}
