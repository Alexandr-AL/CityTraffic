using CityTraffic.Models.GortransPerm.ArrivalTimesVehicles;
using CityTraffic.Models.GortransPerm.FullRouteNew;
using CityTraffic.Models.GortransPerm.RouteTypesTree;
using CityTraffic.Models.GortransPerm.StoppointRoutes;
using Polly;
using Polly.Registry;
using System.Text.Json;

namespace CityTraffic.Services.GortransPerm
{
    public class GortransPermAPI : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipeline _pipeline;

        public GortransPermAPI(HttpClient httpClient, ResiliencePipelineProvider<string> pipelineProvider)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://www.map.gortransperm.ru/json/");

            _pipeline = pipelineProvider.GetPipeline("retry-pipeline");
        }

        private async Task<T> GetInternalAsync<T>(string endpoint, CancellationToken token = default)
        {
            try
            {
                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(endpoint, token);

                if (responseMessage.IsSuccessStatusCode)
                {
                    await using Stream content = await responseMessage.Content.ReadAsStreamAsync(token);

                    return await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token);
                }

                var errorContent = await responseMessage.Content.ReadAsStringAsync(token);

                throw new GortransPermException(message: "Error while executing request to GortransPermAPI",
                                                httpStatusCode: responseMessage.StatusCode,
                                                requestUrl: _httpClient.BaseAddress + endpoint,
                                                responseContent: errorContent);
            }
            catch (Exception ex) when (ex is not GortransPermException)
            {

                throw new GortransPermException(message: "General error when working with GortransPermAPI",
                                                httpStatusCode: System.Net.HttpStatusCode.InternalServerError,
                                                requestUrl: _httpClient.BaseAddress + endpoint,
                                                responseContent: null,
                                                innerException: ex);
            }
        }

        private async Task<T> GetAsync<T>(string endpoint, CancellationToken token = default)
        {
            return await _pipeline.ExecuteAsync(async ct => await GetInternalAsync<T>(endpoint, ct), token);
            //try
            //{
            //    using HttpResponseMessage responseMessage = await _httpClient.GetAsync(endpoint, token);

            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        await using Stream content = await responseMessage.Content.ReadAsStreamAsync(token);

            //        return await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token);
            //    }

            //    var errorContent = await responseMessage.Content.ReadAsStringAsync(token);

            //    throw new GortransPermException(message: "Error while executing request to GortransPermAPI",
            //                                    httpStatusCode: responseMessage.StatusCode,
            //                                    requestUrl: _httpClient.BaseAddress + endpoint,
            //                                    responseContent: errorContent);
            //}
            //catch (Exception ex) when (ex is not GortransPermException)
            //{

            //    throw new GortransPermException(message: "General error when working with GortransPermAPI",
            //                                    httpStatusCode: System.Net.HttpStatusCode.InternalServerError,
            //                                    requestUrl: _httpClient.BaseAddress + endpoint,
            //                                    responseContent: null,
            //                                    innerException: ex);
            //}
        }

        /// <summary>
        /// Список всех видов транспорта
        /// </summary>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<IEnumerable<RouteTypesTree>> GetRouteTypes(DateTime? date = null, CancellationToken token = default)
        {
            string endpointRouteTypesTree = $"route-types-tree/{FormatDate(date)}/";

            return await GetAsync<IEnumerable<RouteTypesTree>>(endpointRouteTypesTree, token);
        }

        /// <summary>
        /// Информация о маршруте
        /// </summary>
        /// <param name="routeId">Id маршрута</param>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<FullRouteNew> GetFullRoute(string routeId, DateTime? date = null, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(routeId)) throw new ArgumentNullException(nameof(routeId), "Invalid route identifier");

            string endpointFullRouteNew = $"full-route-new/{FormatDate(date)}/{routeId}";

            return await GetAsync<FullRouteNew>(endpointFullRouteNew, token);
        }

        /// <summary>
        /// Маршруты остановки
        /// </summary>
        /// <param name="stoppointId">Id остановки</param>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<StoppointRoutes> GetStoppointRoutes(string stoppointId, DateTime? date = null, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(stoppointId)) throw new ArgumentNullException(nameof(stoppointId), "Invalid stoppoint identifier");

            string endpointStoppointRoutes = $"stoppoint-routes/{FormatDate(date)}/{stoppointId}";

            return await GetAsync<StoppointRoutes>(endpointStoppointRoutes, token);
        }

        /// <summary>
        /// Ближайшие прибытия транспорта на остановку
        /// </summary>
        /// <param name="stoppointId">Id остановки</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ArrivalTimesVehicles> GetArrivalTimesVehicles(string stoppointId, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(stoppointId)) throw new ArgumentNullException(nameof(stoppointId), "Invalid stoppoint identifier");

            var timestamp = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();

            string endpointArrivalTimesVehicles = $"arrival-times-vehicles/{stoppointId}?_={timestamp}";

            return await GetAsync<ArrivalTimesVehicles>(endpointArrivalTimesVehicles, token);
        }

        private static string FormatDate(DateTime? date) => 
            (date ?? DateTime.Now).ToString("dd.MM.yyyy");

        public void Dispose()
        {
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
