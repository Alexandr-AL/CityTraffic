using CityTraffic.Models.GortransPerm.FullRouteNew;
using CityTraffic.Models.GortransPerm.RouteTypesTree;

namespace CityTraffic.Services.GortransPerm
{
    public static class GortransPermAPIExtensions
    {
        /// <summary>
        /// Список всех маршрутов
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Child>> GetAllTransportRoutes(this GortransPermAPI gortransPermAPI, CancellationToken token = default)
        {
            IEnumerable<RouteTypesTree> transportTypes = await gortransPermAPI.GetRouteTypes(token: token) ?? Enumerable.Empty<RouteTypesTree>();

            return transportTypes.SelectMany(transportType => transportType.Children);
        }

        /// <summary>
        /// Список всех остановок
        /// </summary>
        /// <param name="transport">Список всех маршрутов</param>
        /// <returns></returns>
        public static async Task<List<TransportStoppoint>> GetAllStoppoints(this GortransPermAPI gortransPermAPI, 
                                                                            IEnumerable<Child> transport,
                                                                            CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(transport);

            HashSet<int> uniqueStoppoints = new HashSet<int>();

            List<TransportStoppoint> result = new List<TransportStoppoint>();

            Lock syncLock = new System.Threading.Lock();

            await Parallel.ForEachAsync(transport, token, async (route, ct) =>
            {
                FullRouteNew routeInfo = await gortransPermAPI.GetFullRoute(route.RouteId, token: ct);
                if (routeInfo is null) return;

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
