﻿using CityTraffic.Models.Base;
using CityTraffic.Models.Entities;
using CityTraffic.Models.GortransPerm.FullRouteNew;
using CityTraffic.Models.GortransPerm.RouteTypesTree;
using CityTraffic.Models.Interfaces;
using System.Text.Json;

namespace CityTraffic.Services
{
    public static class GortransPermAPI
    {
        /// <summary>
        /// Список всех видов транспорта
        /// </summary>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public static async Task<List<RouteTypesTree>> GetRouteTypes(DateTime date = default)
        {
            HttpClient httpClient = new();
            date = date == default ? DateTime.Now : date;

            string uriRouteTypesTree = $"http://www.map.gortransperm.ru/json/route-types-tree/{date:dd.MM.yyyy}/";

            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uriRouteTypesTree);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Stream content = await responseMessage.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<List<RouteTypesTree>>(content);
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
            finally { httpClient.Dispose(); }
        }

        /// <summary>
        /// Информация о маршруте
        /// </summary>
        /// <param name="routeId">Id маршрута</param>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public static async Task<FullRouteNew> GetFullRoute(string routeId, DateTime date = default)
        {
            HttpClient httpClient = new();
            date = date == default ? DateTime.Now : date;

            string uriFullRouteNew = $"http://www.map.gortransperm.ru/json/full-route-new/{date:dd.MM.yyyy}/{routeId}";

            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uriFullRouteNew);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Stream content = await responseMessage.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<FullRouteNew>(content);
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
            finally { httpClient.Dispose(); }
        }

        /// <summary>
        /// Список всех маршрутов
        /// </summary>
        /// <returns></returns>
        public static async Task<List<TransportRoute>> GetAllTransportRoutes()
        {
            List<RouteTypesTree> transportTypes = await GetRouteTypes();
            if (transportTypes is null) return default;
            return transportTypes.SelectMany(transportType => transportType.Children.ToList<TransportRoute>()).ToList();
        }

        /// <summary>
        /// Список всех остановок
        /// </summary>
        /// <param name="transport">Список всех маршрутов</param>
        /// <returns></returns>
        public static async Task<List<Stoppoint>> GetAllStoppoints(IEnumerable<ITransportRoute> transport)
        {
            if (transport is null) return default;

            List<string> allRouteIdInTransport = transport.Select(t => t.RouteId).ToList();

            List<Stoppoint> allStoppoints = new();

            foreach (var routeId in allRouteIdInTransport)
            {
                FullRouteNew routeInfo = await GetFullRoute(routeId);

                if (routeInfo is not null)
                {
                    allStoppoints.AddRange(routeInfo.FwdStoppoints
                        .Where(stoppoint => !allStoppoints
                            .Select(sp => sp.StoppointId)
                            .Contains(stoppoint.StoppointId)));

                    allStoppoints.AddRange(routeInfo.BkwdStoppoints
                        .Where(stoppoint => !allStoppoints
                            .Select(sp => sp.StoppointId)
                            .Contains(stoppoint.StoppointId)));
                }
            }
            return allStoppoints;
        }
    }
}
