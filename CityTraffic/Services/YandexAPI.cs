using CityTraffic.Models.YandexTimetable.StationList;
using System.Diagnostics;
using System.Text.Json;

namespace CityTraffic.Services
{
    public static class YandexAPI
    {
        public static async Task<StationList> GetStationListYA(HttpClient httpClient = default)
        {
            httpClient ??= new HttpClient();

            string APIKey = "982b9621-2fd5-4d65-95c2-f14eb2a241c4";
            string lang = "ru_RU";
            string format = "json";

            string uriStationList = $"https://api.rasp.yandex.net/v3.0/stations_list/?apikey={APIKey}&lang={lang}&format={format}";
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uriStationList);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Stream content = await responseMessage.Content.ReadAsStreamAsync();

                    return await JsonSerializer.DeserializeAsync<StationList>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { httpClient.Dispose(); }

            return default;
        }
    }
}
