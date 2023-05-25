using CityTraffic.DAL;
using CityTraffic.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public MainPageViewModel(CityTrafficDB dB)
        {
            _dB = dB;
        }

        [RelayCommand]
        private async void Initialize()
        {
            await _dB.InitDB();
        }

        [RelayCommand]
        private async void Update()
        {
            var result = await _dB.UpdateDB();

            await Shell.Current.DisplayAlert($"Updated {result.Item1} entites in {result.Item2} sec.", 
                                             $"Count Transport routes {_dB.TransportRoutes.Count()}\n" +
                                             $"Count Stationpoints {_dB.Stoppoints.Count()}", "OK");
        }

        [RelayCommand]
        private async void ClearTables()
        {
            _dB.TransportRoutes.ExecuteDelete();
            _dB.Stoppoints.ExecuteDelete();

            await Shell.Current.DisplayAlert("Tables cleared", "", "OK");
        }
    }
}