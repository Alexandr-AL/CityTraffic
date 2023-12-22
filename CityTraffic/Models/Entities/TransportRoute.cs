using CityTraffic.Models.Base;
using CityTraffic.Models.Entities.Interfaces;

namespace CityTraffic.Models.Entities
{
    public class TransportRoute : BaseTransportRoute, IEntity
    {
        public int Id { get; }

        public FavoritesTransportRoute FavoritesTransportRoute { get; set; }
    }
}
