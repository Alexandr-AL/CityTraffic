using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.ErrorHandler;
using CityTraffic.Services.FavoriteService;
using CityTraffic.Services.ShowDataGortransService;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using UraniumUI.Dialogs;

namespace CityTraffic.ViewModels
{
    public partial class FavoriteStoppointsViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly IShowDataGortransService _showDataService;
        private readonly IFavoriteService _favoriteService;
        private readonly IDialogService _dialogService;

        public FavoriteStoppointsViewModel(CityTrafficDB cityTrafficDB,
                                           IShowDataGortransService showDataGortransService,
                                           IFavoriteService favoriteService,
                                           IDialogService dialogService,
                                           IErrorHandler errorHandler) : base(errorHandler)
        {
            _dB = cityTrafficDB;
            _showDataService = showDataGortransService;
            _favoriteService = favoriteService;
            _dialogService = dialogService;

            LoadFavoriteStoppoints();

            WeakReferenceMessenger.Default.Register<FavoriteStoppointChangedMessage>(this, FavoriteStoppointMessageHandler);
            WeakReferenceMessenger.Default.Register<DataSyncServiceChangedMessage>(this, DataSyncServiceMessageHandler);
        }

        [ObservableProperty]
        public partial ObservableCollection<StoppointEntity> FavoriteStoppoints { get; set; }

        [RelayCommand]
        public void LoadFavoriteStoppoints()
        {
            FavoriteStoppoints = _dB.Stoppoints.Include(s => s.Routes).Where(s => s.IsFavorite).ToObservableCollection();
        }

        [RelayCommand]
        public async Task ToggleFavoriteStoppointAsync(StoppointEntity stoppoint)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(stoppoint);

                if (!await _dialogService.ConfirmAsync("", $"Удаление из избранного:\n{stoppoint.StoppointName} {stoppoint.Note}")) return;

                try
                {
                    IsBusy = true;

                    CancellationToken token = new CancellationTokenSource().Token;
                    await _favoriteService.ToggleFavoriteAsync(stoppoint, token);

                    FavoriteStoppoints.Remove(stoppoint);
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        [RelayCommand]
        public async Task ToggleRouteAsync(TransportRouteEntity route)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(route);

                CancellationToken token = new CancellationTokenSource().Token;
                await _favoriteService.ToggleFavoriteAsync(route, token);
            });
        }

        [RelayCommand]
        public async Task ArrivalTimesVehicles(StoppointEntity stoppointEntity)
        {
            await _errorHandler.SafeExecuteAsync(async() =>
            {
                await _showDataService.ShowArrivalTimesVehicles(stoppointEntity);
            });
        }

        private void FavoriteStoppointMessageHandler(object recipient, FavoriteStoppointChangedMessage message)
        {
            if (IsBusy) return;

            ArgumentNullException.ThrowIfNull(message);

            StoppointEntity sp = _dB.Stoppoints.FirstOrDefault(s => s.StoppointId == message.Value);

            ArgumentNullException.ThrowIfNull(sp);

            if (sp.IsFavorite)
                FavoriteStoppoints.Insert(0, sp);
            else
                FavoriteStoppoints.Remove(sp);
        }

        private void DataSyncServiceMessageHandler(object recipient, DataSyncServiceChangedMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.Value == 0) return;

            LoadFavoriteStoppoints();
        }
    }
}
