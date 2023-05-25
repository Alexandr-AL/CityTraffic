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

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not Stoppoint other) return false;

            return StoppointId == other.StoppointId &&
                   StoppointName == other.StoppointName &&
                   Location == other.Location &&
                   Note == other.Note;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
