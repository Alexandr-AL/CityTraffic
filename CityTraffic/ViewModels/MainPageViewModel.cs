using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public MainPageViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            _dB.InitDB();
            
            FavoritesTR = new(_dB.FavoritesTransportRoutes);
            FavoritesSp = new(_dB.FavoritesStoppoints);
        }

        [ObservableProperty]
        private ObservableCollection<FavoritesTransportRoute> _favoritesTR;

        [ObservableProperty]
        private ObservableCollection<FavoritesStoppoint> _favoritesSp;

        [RelayCommand]
        private async void Update()
        {
            var result = await _dB.UpdateDB();

            await Shell.Current.DisplayAlert($"Updated {result.Item1} entities in {result.Item2} sec.", 
                                             $"Count Transport routes {_dB.TransportRoutes.Count()}\n" +
                                             $"Count Stoppoints {_dB.Stoppoints.Count()}", "OK");
        }

        private void DeleteFavoritesTR(FavoritesTransportRoute favoritesTransportRoute)
        {
            _dB.FavoritesTransportRoutes.Remove(favoritesTransportRoute);
            _dB.SaveChanges();
        }
    }
}