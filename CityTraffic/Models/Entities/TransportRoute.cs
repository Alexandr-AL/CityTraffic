using CityTraffic.Models.Interfaces;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.Entities
{
    public class TransportRoute : IEntity, ITransportRoute
    {
        public int Id { get; }

        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        public FavoritesTransportRoute FavoritesTransportRoute { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not TransportRoute other) return false;

            return RouteId == other.RouteId
                && RouteNumber == other.RouteNumber
                && Title == other.Title
                && RouteTypeId == other.RouteTypeId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
