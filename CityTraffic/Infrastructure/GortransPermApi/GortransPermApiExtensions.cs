using CityTraffic.Models.GortransPerm.FullRouteNew;
using CityTraffic.Models.GortransPerm.RouteTypesTree;

namespace CityTraffic.Infrastructure.GortransPermApi
{
    public static class GortransPermApiExtensions
    {
        /// <summary>
        /// Список всех маршрутов
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Child>> GetAllTransportRoutesAsync(this GortransPermApi gortransPermAPI, 
                                                                                     DateTime? date = null,
                                                                                     CancellationToken token = default)
        {
            IEnumerable<RouteTypesTree> transportTypes = await gortransPermAPI.GetRouteTypesAsync(date, token) 
                ?? throw new NullReferenceException();

            return transportTypes.SelectMany(transportType => transportType.Children);
        }

        /// <summary>
        /// Список всех остановок
        /// </summary>
        /// <param name="transport">Список всех маршрутов</param>
        /// <returns></returns>
        public static async Task<List<TransportStoppoint>> GetAllStoppointsAsync(this GortransPermApi gortransPermAPI,
                                                                                      IEnumerable<Child> transport,
                                                                                      DateTime? date = null,
                                                                                      CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(transport);

            HashSet<int> uniqueStoppoints = new HashSet<int>();

            List<TransportStoppoint> result = new List<TransportStoppoint>();

            Lock syncLock = new Lock();

            await Parallel.ForEachAsync(transport, token, async (route, ct) =>
            {
                FullRouteNew routeInfo = await gortransPermAPI.GetFullRouteAsync(route.RouteId, date, token: ct) 
                    ?? throw new NullReferenceException();

                lock (syncLock)
                {
                    foreach (var transportStoppoint in routeInfo.FwdStoppoints.Concat(routeInfo.BkwdStoppoints))
                    {
                        if (uniqueStoppoints.Add(transportStoppoint.StoppointId))
                            result.Add(transportStoppoint);
                    }
                }
            });

            return result;
        }
    }
}
