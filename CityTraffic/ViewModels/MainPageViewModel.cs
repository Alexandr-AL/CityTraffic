using CityTraffic.Models.StationList;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.System;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;

        public StationList StationList { get; set; }

        public MainPageViewModel()
        {
            _httpClient = new();
        }

        [RelayCommand]
        private async Task<StationList> GetStationList()
        {
            Uri uri = new($"https://api.rasp.yandex.net/v3.0/stations_list/?apikey={App.APIKey}&lang=ru_RU&format=json");
            try
            {
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(uri);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStreamAsync();

                    StationList = await JsonSerializer.DeserializeAsync<StationList>(content);

                    return StationList;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return default;
        }

        [RelayCommand]
        private async Task LoadFromFile()
        {
            string path = "C:\\Users\\Администратор\\Desktop\\StationList.json";

            using var reader = File.OpenText(path);

            var text = reader.ReadToEnd();

            StationList = JsonSerializer.Deserialize<StationList>(text);
        }
    }
}