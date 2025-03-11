using CityTraffic.DAL;
using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Models;
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
using System.Text;
using UraniumUI.Dialogs;

namespace CityTraffic.ViewModels
{
    public partial class FavoriteTransportRoutesViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermApi _api;
        private readonly IFavoriteService _favoriteService;
        private readonly IDialogService _dialogService;

        public FavoriteTransportRoutesViewModel(CityTrafficDB cityTrafficDB,
                                                GortransPermApi gortransPermApi,
                                                IErrorHandler errorHandler,
                                                IFavoriteService favoriteService,
                                                IDialogService dialogService) : base(errorHandler)
        {
            _dB = cityTrafficDB;
            this._api = gortransPermApi;
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
            if (route is null) return;

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
        }

        [RelayCommand]
        public async Task ToggleStoppointAsync(StoppointEntity stoppoint)
        {
            if (stoppoint is null) return;

            CancellationToken token = new CancellationTokenSource().Token;
            await _favoriteService.ToggleFavoriteAsync(stoppoint, token);
        }

        [RelayCommand]
        public async Task TimeTableH(object idS)
        {
            if (idS is null) return;

            RouteIdStoppointId idTrSp = idS as RouteIdStoppointId;

            Models.GortransPerm.TimeTableH.TimeTableH result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _api.GetTimeTableHAsync(idTrSp.RouteId, idTrSp.StoppointId);
            });

            if (result is null)
            {
                await Shell.Current.DisplayPopupAsync("Данные отсутствуют.");
                return;
            }

            StoppointEntity stoppoint = await _dB.Stoppoints.FindAsync(idTrSp.StoppointId);

            string popupMessage = $"{result.Date}\n{stoppoint.StoppointName}\n  ({stoppoint.Note})\n№{result.Route.RouteNumber} ({result.Route.RouteName})";

            foreach (var timeTable in result.TimeTable)
            {
                foreach (var stopTime in timeTable.StopTimes)
                {
                    popupMessage += $"""

                                    {stopTime.ScheduledTime}
                                    """;
                }
            }
            await Shell.Current.DisplayPopupAsync(popupMessage);
        }

        private void FavoriteRouteMessageHandler(object recipient, FavoriteRouteChangedMessage message)
        {
            if (IsBusy) return;

            ArgumentNullException.ThrowIfNull(message);

            TransportRouteEntity tr = _dB.TransportRoutes.FirstOrDefault(t => t.RouteId == message.Value);

            if (tr is null) return;

            if (tr.IsFavorite)
                FavoriteTransportRoutes.Insert(0, tr);
            else
                FavoriteTransportRoutes.Remove(tr);
        }

        private void DataSyncServiceMessageHandler(object recipient, DataSyncServiceChangedMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.Value == 0) return;

            LoadFavoriteRoutes();
        }
    }
}
