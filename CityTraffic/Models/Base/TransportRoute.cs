using CityTraffic.Models.Interfaces;

namespace CityTraffic.Models.Base
{
    public class TransportRoute : ITransportRoute
    {
        public virtual string RouteId { get; set; }

        public virtual string RouteNumber { get; set; }

        public virtual int RouteTypeId { get; set; }

        public virtual string Title { get; set; }
    }
}
