﻿using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CityTraffic.Services.DialogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CityTraffic.ViewModels
{
    public partial class StoppointListViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public StoppointListViewModel(CityTrafficDB dB, IDialogService dialogService) : base(dialogService)
        {
            _dB = dB;
            _stoppoints = new(_dB.Stoppoints.OrderBy(x => x.StoppointName).AsEnumerable());
        }

        [ObservableProperty]
        private ObservableCollection<EntityStoppoint> _stoppoints;

        [RelayCommand]
        private async Task FavoritesChanged()
        {
            await Shell.Current.DisplayAlert("test", "test FavoritesChanged", "OK");
        }
    }
}
