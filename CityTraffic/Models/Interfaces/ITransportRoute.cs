namespace CityTraffic.Models.Interfaces
{
    public interface ITransportRoute
    {
        string RouteId { get; set; }

        string RouteNumber { get; set; }

        public int RouteTypeId { get; set; }

        string Title { get; set; }
    }
}
