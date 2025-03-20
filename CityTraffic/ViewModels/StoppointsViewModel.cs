using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.ErrorHandler;
using CityTraffic.Services.FavoriteService;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CityTraffic.ViewModels
{
    public partial class StoppointsViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly IFavoriteService _favoriteService;

        public StoppointsViewModel(CityTrafficDB dB,
                                   IErrorHandler errorHandler,
                                   IFavoriteService favoriteService) : base(errorHandler)
        {
            _dB = dB;
            _favoriteService = favoriteService;

            LoadStoppoints();

            WeakReferenceMessenger.Default.Register<DataSyncServiceChangedMessage>(this, DataSyncServiceMessageHandler);
        }

        [ObservableProperty]
        public partial ObservableCollection<StoppointEntity> Stoppoints { get; set; }

        [RelayCommand]
        public async Task ToggleFavorite(StoppointEntity stoppoint)
        {
            await _favoriteService.ToggleFavoriteAsync(stoppoint);
        }

        [RelayCommand]
        public void LoadStoppoints()
        {
            Stoppoints = _dB.Stoppoints.OrderBy(s => s.StoppointId).Include(s => s.Routes).ToObservableCollection();
        }

        private async void DataSyncServiceMessageHandler(object recipient, DataSyncServiceChangedMessage message)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(message);

                if (message.Value == 0) return;

                LoadStoppoints();
            });
        }
    }
}
