using CityTraffic.DAL;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Models.Entities;
using CityTraffic.Models.GortransPerm.FullRouteNew;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CityTraffic.Services.DataSyncService
{
    public class DataSyncService : IDataSyncService
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermApi _api;

        public DataSyncService(CityTrafficDB dB, GortransPermApi api)
        {
            _dB = dB;
            _api = api;
        }

        private async Task<(int, int)> InitializeDatabaseAsync(CancellationToken token = default)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            if (await _dB.TransportRoutes.AnyAsync(token) || await _dB.Stoppoints.AnyAsync(token))
            {
                timer.Stop();
                return (0, timer.Elapsed.Seconds);
            }

            DateTime currentDate = DateTime.Now;

            Lock syncLock = new();

            HashSet<int> existingStopIds = new HashSet<int>();

            IEnumerable<Models.GortransPerm.RouteTypesTree.RouteTypesTree> transportTypes =
                await _api.GetRouteTypesAsync(currentDate, token);

            IEnumerable<Models.GortransPerm.RouteTypesTree.Child> allTransportRoutes =
                transportTypes.SelectMany(t => t.Children);

            await Parallel.ForEachAsync(allTransportRoutes, token, async (route, ct) =>
            {
                FullRouteNew fullRoute = await _api.GetFullRouteAsync(route.RouteId, currentDate, token);

                lock (syncLock)
                {
                    IEnumerable<TransportStoppoint> routeStoppoints = fullRoute.FwdStoppoints
                                                                               .Concat(fullRoute.BkwdStoppoints)
                                                                               .DistinctBy(s => s.StoppointId);

                    _dB.TransportRoutes.Add(new TransportRouteEntity
                    {
                        RouteId = route.RouteId,
                        RouteNumber = route.RouteNumber,
                        Title = route.Title,
                        RouteTypeId = route.RouteTypeId,
                        Stoppoints = routeStoppoints
                            .Select(s => !existingStopIds.Contains(s.StoppointId)
                                ? MapToStoppointEntity(s)
                                : _dB.Stoppoints.Find(s.StoppointId))
                            .ToList()
                    });

                    foreach (var stoppoint in routeStoppoints)
                        existingStopIds.Add(stoppoint.StoppointId);
                }
            });

            timer.Stop();

            (int countUpdated, int seconds) result = (await _dB.SaveChangesAsync(token), timer.Elapsed.Seconds);

            WeakReferenceMessenger.Default.Send(new DataSyncServiceChangedMessage(result.countUpdated));

            return result;
        }

        public async Task<(int, int)> UpdateDatabaseAsync(CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(_dB);
            ArgumentNullException.ThrowIfNull(_api);

            await _dB.Database.MigrateAsync(token);

            if (!await _dB.TransportRoutes.AnyAsync(token) && !await _dB.Stoppoints.AnyAsync(token))
                return await InitializeDatabaseAsync(token);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            DateTime currentDate = DateTime.Now;

            Lock objLock = new Lock();

            HashSet<string> nonExistentRouteIds = _dB.TransportRoutes.Select(t => t.RouteId).ToHashSet();
            HashSet<int> nonExistentStoppointIds = _dB.Stoppoints.Select(s => s.StoppointId).ToHashSet();

            IEnumerable<Models.GortransPerm.RouteTypesTree.RouteTypesTree> transportTypes =
                await _api.GetRouteTypesAsync(currentDate, token);

            IEnumerable<Models.GortransPerm.RouteTypesTree.Child> allTransportRoutes =
                transportTypes.SelectMany(t => t.Children);

            await Parallel.ForEachAsync(allTransportRoutes, token, async (route, ct) =>
            {
                FullRouteNew fullRoute = await _api.GetFullRouteAsync(route.RouteId, currentDate, token);

                IEnumerable<TransportStoppoint> routeStoppoints = fullRoute.FwdStoppoints
                                                                           .Concat(fullRoute.BkwdStoppoints)
                                                                           .DistinctBy(s => s.StoppointId);
                lock (objLock)
                {
                    TransportRouteEntity existingRoute = _dB.TransportRoutes
                                                            .Include(t => t.Stoppoints)
                                                            .SingleOrDefault(t => t.RouteId == route.RouteId);

                    if (existingRoute is null)
                    {
                        _dB.TransportRoutes.Add(new TransportRouteEntity
                        {
                            RouteId = route.RouteId,
                            RouteNumber = route.RouteNumber,
                            Title = route.Title,
                            RouteTypeId = route.RouteTypeId,
                            Stoppoints = routeStoppoints
                                .Select(s =>
                                {
                                    StoppointEntity result = _dB.Stoppoints.Find(s.StoppointId);

                                    if (result is null)
                                        return MapToStoppointEntity(s);

                                    nonExistentStoppointIds.Remove(s.StoppointId);
                                    return result;
                                })
                                .ToList()
                        });
                    }
                    else
                    {
                        nonExistentRouteIds.Remove(existingRoute.RouteId);

                        foreach (var stoppoint in routeStoppoints)
                        {
                            if (existingRoute.Stoppoints.SingleOrDefault(s => s.StoppointId == stoppoint.StoppointId) is null)
                                existingRoute.Stoppoints.Add(
                                    _dB.Stoppoints.Find(stoppoint.StoppointId)
                                    ?? MapToStoppointEntity(stoppoint));
                            else
                                nonExistentStoppointIds.Remove(stoppoint.StoppointId);
                        }
                    }
                }
            });

            foreach (var routeIdToDelete in nonExistentRouteIds)
                _dB.TransportRoutes.Remove(_dB.TransportRoutes.Find(routeIdToDelete));

            foreach (var stoppointIdToDelete in nonExistentStoppointIds)
                _dB.Stoppoints.Remove(_dB.Stoppoints.Find(stoppointIdToDelete));

            timer.Stop();

            (int countUpdated, int seconds) result = (await _dB.SaveChangesAsync(token), timer.Elapsed.Seconds);

            WeakReferenceMessenger.Default.Send(new DataSyncServiceChangedMessage(result.countUpdated));

            return result;
        }

        private static StoppointEntity MapToStoppointEntity(TransportStoppoint transportStoppoint) => new StoppointEntity
        {
            StoppointId = transportStoppoint.StoppointId,
            StoppointName = transportStoppoint.StoppointName,
            Location = new Models.Entities.Location(transportStoppoint.Location),
            Note = transportStoppoint.Note
        };
    }
}
