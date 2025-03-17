using CityTraffic.Models.Entities;

namespace CityTraffic.Services.ShowDataGortransService
{
    public interface IShowDataGortransService
    {
        Task ShowBoards(CancellationToken token = default);

        Task ShowArrivalTimesVehicles(StoppointEntity stoppoint, CancellationToken token = default);

        Task ShowMovingAutos(string routeId, CancellationToken token = default);

        Task ShowNewsLinks(CancellationToken token = default);

        Task ShowSearch(string query, CancellationToken token = default);

        Task ShowStoppointTimetable(StoppointEntity stoppoint, CancellationToken token = default);

        Task ShowTimeTableH(string routeId, int stoppointId, CancellationToken token = default);
    }
}