namespace CityTraffic.Models.Entities
{
    public class EntityTransportRoute : Base.TransportRoute
    {
        //public override string RouteId { get; set; }

        //public override string RouteNumber { get; set; }

        //public override int RouteTypeId { get; set; }

        //public override string Title { get; set; }

        public bool FavoriteTransportRoute { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not EntityTransportRoute other) return false;

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
