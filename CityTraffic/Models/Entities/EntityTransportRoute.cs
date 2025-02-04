namespace CityTraffic.Models.Entities
{
    public class EntityTransportRoute
    {
        public string RouteId { get; set; }

        public string RouteNumber { get; set; }

        public int RouteTypeId { get; set; }

        public string Title { get; set; }

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
