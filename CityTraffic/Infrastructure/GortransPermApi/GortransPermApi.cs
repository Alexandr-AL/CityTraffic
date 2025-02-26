using CityTraffic.Models.GortransPerm.ArrivalTimesVehicles;
using CityTraffic.Models.GortransPerm.Board;
using CityTraffic.Models.GortransPerm.FullRouteNew;
using CityTraffic.Models.GortransPerm.MovingAutos;
using CityTraffic.Models.GortransPerm.News;
using CityTraffic.Models.GortransPerm.RouteTypesTree;
using CityTraffic.Models.GortransPerm.Search;
using CityTraffic.Models.GortransPerm.StoppointRoutes;
using CityTraffic.Models.GortransPerm.StoppointTimetable;
using CityTraffic.Models.GortransPerm.TimeTableH;
using System.Text.Json;

namespace CityTraffic.Infrastructure.GortransPermApi
{
    public class GortransPermApi
    {
        private readonly HttpClient _httpClient;

        public GortransPermApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://www.map.gortransperm.ru/json/");
        }

        private async Task<T> GetAsync<T>(string endpoint, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(endpoint, token);

                if (responseMessage.IsSuccessStatusCode)
                {
                    await using Stream content = await responseMessage.Content.ReadAsStreamAsync(token);

                    return await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token);
                }

                string errorContent = await responseMessage.Content.ReadAsStringAsync(token);

                throw new GortransPermApiException(message: "Ошибка при выполнении запроса к GortransPermApi",
                                                   httpStatusCode: responseMessage.StatusCode,
                                                   requestUrl: _httpClient.BaseAddress + endpoint,
                                                   responseContent: errorContent);
            }
            catch (Exception ex) when (ex is not GortransPermApiException)
            {
                throw new GortransPermApiException(message: "Общая ошибка при работе с GortransPermApi",
                                                   httpStatusCode: System.Net.HttpStatusCode.InternalServerError,
                                                   requestUrl: _httpClient.BaseAddress + endpoint,
                                                   responseContent: null,
                                                   innerException: ex);
            }
        }

        /// <summary>
        /// Список всех видов транспорта
        /// </summary>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<IEnumerable<RouteTypesTree>> GetRouteTypesAsync(DateTime? date = null, CancellationToken token = default)
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
        public async Task<FullRouteNew> GetFullRouteAsync(string routeId, DateTime? date = null, CancellationToken token = default)
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
        public async Task<StoppointRoutes> GetStoppointRoutesAsync(int stoppointId, DateTime? date = null, CancellationToken token = default)
        {
            string endpointStoppointRoutes = $"stoppoint-routes/{FormatDate(date)}/{stoppointId}";

            return await GetAsync<StoppointRoutes>(endpointStoppointRoutes, token);
        }

        /// <summary>
        /// Ближайшие прибытия транспорта на остановку
        /// </summary>
        /// <param name="stoppointId">Id остановки</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ArrivalTimesVehicles> GetArrivalTimesVehiclesAsync(int stoppointId, CancellationToken token = default)
        {
            string endpointArrivalTimesVehicles = $"arrival-times-vehicles/{stoppointId}?_={UnixTimestampNow()}";

            return await GetAsync<ArrivalTimesVehicles>(endpointArrivalTimesVehicles, token);
        }

        /// <summary>
        /// Расписание движения транспорта по остановке
        /// </summary>
        /// <param name="stoppointId">Id остановки</param>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<IEnumerable<StoppointTimetable>> GetStoppointTimetableAsync(int stoppointId, DateTime? date = null, CancellationToken token = default)
        {
            string endpointStoppointTimetable = $"stoppoint-time-table/{FormatDate(date)}/{stoppointId}?_={UnixTimestampNow()}";

            return await GetAsync<IEnumerable<StoppointTimetable>>(endpointStoppointTimetable, token);
        }

        /// <summary>
        /// Расписание движения маршрута по остановке
        /// </summary>
        /// <param name="routeId">Id маршрута</param>
        /// <param name="stoppointId">Id остановки</param>
        /// <param name="date">Информация на конкретную дату, по умолчанию DateTime.Now</param>
        /// <returns></returns>
        public async Task<TimeTableH> GetTimeTableHAsync(string routeId, int stoppointId, DateTime? date = null, CancellationToken token = default)
        {
            string endpointTimeTableH = $"time-table-h/{FormatDate(date)}/{routeId}/{stoppointId}";

            return await GetAsync<TimeTableH>(endpointTimeTableH, token);
        }

        /// <summary>
        /// Поиск маршрутов и остановок
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <returns></returns>
        public async Task<Search> GetSearchAsync(string query, CancellationToken token = default)
        {
            string endpointSearch = $"search?q={query}";

            return await GetAsync<Search>(endpointSearch, token);
        }

        /// <summary>
        /// Движение транспорта онлайн
        /// </summary>
        /// <param name="routeId">Id маршрута</param>
        /// <returns></returns>
        public async Task<MovingAutos> GetMovingAutosAsync(string routeId, CancellationToken token = default)
        {
            string endpointMovingAutos = $"get-moving-autos/-{routeId}-?_={UnixTimestampNow()}";

            return await GetAsync<MovingAutos>(endpointMovingAutos, token);
        }

        /// <summary>
        /// Новости
        /// </summary>
        /// <returns></returns>
        public async Task<News> GetNewsLinksAsync(CancellationToken token = default)
        {
            string endpointNewsLinks = $"news-links?_={UnixTimestampNow()}";

            return await GetAsync<News>(endpointNewsLinks, token);
        }

        /// <summary>
        /// Список остановок где установлены информационные табло
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Board>> GetBoardsAsync(CancellationToken token = default)
        {
            string endpointBoards = $"boards";

            return await GetAsync<IEnumerable<Board>>(endpointBoards, token);
        }


        private static string FormatDate(DateTime? date) =>
            (date ?? DateTime.Now).ToString("dd.MM.yyyy");

        private static long UnixTimestampNow() =>
            TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();
    }
}
