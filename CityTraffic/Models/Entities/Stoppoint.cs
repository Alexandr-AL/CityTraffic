using CityTraffic.Models.Base;
using CityTraffic.Models.Entities.Interfaces;

namespace CityTraffic.Models.Entities
{
    public class Stoppoint : BaseStoppoint, IEntity
    {
        public int Id { get;}

        public FavoritesStoppoint FavoritesStoppoint { get; set; }
    }
}
