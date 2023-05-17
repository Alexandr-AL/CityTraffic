using CityTraffic.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CityTraffic.Models.Entities
{
    public class TransportRoute : IEntity, ITransportRoute
    {
        public int Id { get; }
        public string? RouteId { get; set; }
        public string? RouteNumber { get; set; }
        public string? Title { get; set; }
        public int RouteTypeId { get; set; }
        public bool Favorites { get; set; }
    }
}
