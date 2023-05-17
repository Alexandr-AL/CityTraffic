﻿namespace CityTraffic.Models.Interfaces
{
    public interface ITransportRoute
    {
        string RouteId { get; set; }
        string RouteNumber { get; set; }
        string Title { get; set; }
        int RouteTypeId { get; set; }
    }
}