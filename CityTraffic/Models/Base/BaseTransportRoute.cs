using CityTraffic.Models.Base.Interfaces;

namespace CityTraffic.Models.Base
{
    public abstract class BaseTransportRoute : ITransportRoute
    {
        public virtual string RouteId { get; set; }

        public virtual string RouteNumber { get; set; }
        
        public virtual string Title { get; set; }

        public virtual int RouteTypeId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not BaseTransportRoute other) return false;

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
