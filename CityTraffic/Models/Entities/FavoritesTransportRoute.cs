namespace CityTraffic.Models.Entities
{
    public class FavoritesTransportRoute : IEntity
    {
        public int Id { get; }
        public int TransportRouteId { get; set; }
        public TransportRoute TransportRoute { get; set; }
    }
}
