using CityTraffic.Models.GortransPrem.FullRouteNew;
using CityTraffic.Models.GortransPrem.RouteTypesTree;
using System.Text.Json;

namespace CityTraffic.Services
{
    public static class GortransPermAPI
    {
        public static async Task<List<TransportType>> GetRouteTypes(HttpClient httpClient = default, DateTime date = default)
        {
            httpClient ??= new HttpClient();
            date = date == default ? DateTime.Now : date;

            string uriRouteTypesTree = $"http://www.map.gortransperm.ru/json/route-types-tree/{date:dd.MM.yyyy}/";

            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uriRouteTypesTree);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Stream content = await responseMessage.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<List<TransportType>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return default;
        }

        public static async Task<RouteInfo> GetFullRoute(string routeId, HttpClient httpClient = default, DateTime date = default)
        {
            httpClient ??= new HttpClient();
            date = date == default ? DateTime.Now : date;

            string uriFullRouteNew = $"http://www.map.gortransperm.ru/json/full-route-new/{date:dd.MM.yyyy}/{routeId}";

            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uriFullRouteNew);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Stream content = await responseMessage.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<RouteInfo>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return default;
        }
    }
}
