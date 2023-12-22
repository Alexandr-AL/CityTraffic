using CityTraffic.Models.Entities.Interfaces;

namespace CityTraffic.Models.Entities
{
    public class FavoritesStoppoint : IEntity
    {
        public int Id { get; }
        public int StoppointId { get; set; }
        public Stoppoint Stoppoint { get; set; }
    }
}
