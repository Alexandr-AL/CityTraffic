using CityTraffic.DAL;
using CityTraffic.Models;
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
using System.Threading.Tasks;
using UraniumUI.Dialogs;

namespace CityTraffic.ViewModels
{
    public partial class FavoriteTransportRoutesViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly IShowDataGortransService _showDataService;
        private readonly IFavoriteService _favoriteService;
        private readonly IDialogService _dialogService;

        public FavoriteTransportRoutesViewModel(CityTrafficDB cityTrafficDB,
                                                IShowDataGortransService showDataGortransService,
                                                IErrorHandler errorHandler,
                                                IFavoriteService favoriteService,
                                                IDialogService dialogService) : base(errorHandler)
        {
            _dB = cityTrafficDB;
            _showDataService = showDataGortransService;
            _favoriteService = favoriteService;
            _dialogService = dialogService;
            LoadFavoriteRoutes();

            WeakReferenceMessenger.Default.Register<FavoriteRouteChangedMessage>(this, FavoriteRouteMessageHandler);
            WeakReferenceMessenger.Default.Register<DataSyncServiceChangedMessage>(this, DataSyncServiceMessageHandler);
        }

        [ObservableProperty]
        public partial ObservableCollection<TransportRouteEntity> FavoriteTransportRoutes { get; set; }

        [RelayCommand]
        public void LoadFavoriteRoutes()
        {
            FavoriteTransportRoutes = _dB.TransportRoutes.Include(t => t.Stoppoints).Where(tr => tr.IsFavorite).ToObservableCollection();
        }

        [RelayCommand]
        public async Task ToggleFavoriteRouteAsync(TransportRouteEntity route)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(route);

                if (!await _dialogService.ConfirmAsync("", $"Удаление из избранного:\n{route.Title}")) return;

                try
                {
                    IsBusy = true;

                    CancellationToken token = new CancellationTokenSource().Token;
                    await _favoriteService.ToggleFavoriteAsync(route, token);

                    FavoriteTransportRoutes.Remove(route);
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        [RelayCommand]
        public async Task ToggleStoppointAsync(StoppointEntity stoppoint)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(stoppoint);

                CancellationToken token = new CancellationTokenSource().Token;
                await _favoriteService.ToggleFavoriteAsync(stoppoint, token);
            });
        }

        [RelayCommand]
        public async Task TimeTableH(object idS)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(idS);

                RouteIdStoppointId routeIdStoppointId = idS as RouteIdStoppointId;

                await _showDataService.ShowTimeTableH(routeIdStoppointId.RouteId, routeIdStoppointId.StoppointId);
            });
        }

        private async void FavoriteRouteMessageHandler(object recipient, FavoriteRouteChangedMessage message)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                if (IsBusy) return;

                ArgumentNullException.ThrowIfNull(message);

                TransportRouteEntity tr = _dB.TransportRoutes.FirstOrDefault(t => t.RouteId == message.Value);

                ArgumentNullException.ThrowIfNull(tr);

                if (tr.IsFavorite)
                    FavoriteTransportRoutes.Insert(0, tr);
                else
                    FavoriteTransportRoutes.Remove(tr);
            });
        }

        private async void DataSyncServiceMessageHandler(object recipient, DataSyncServiceChangedMessage message)
        {
            await _errorHandler.SafeExecuteAsync(async () =>
            {
                ArgumentNullException.ThrowIfNull(message);

                if (message.Value == 0) return;

                LoadFavoriteRoutes();
            });
        }
    }
}
