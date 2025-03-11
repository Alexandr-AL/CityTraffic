using CityTraffic.DAL;
using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
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
    public partial class FavoriteStoppointsViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermApi _api;
        private readonly IFavoriteService _favoriteService;
        private readonly IDialogService _dialogService;

        public FavoriteStoppointsViewModel(CityTrafficDB cityTrafficDB,
                                           GortransPermApi gortransPermApi,
                                           IFavoriteService favoriteService,
                                           IDialogService dialogService,
                                           IErrorHandler errorHandler) : base(errorHandler)
        {
            _dB = cityTrafficDB;
            _api = gortransPermApi;
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
            if (stoppoint is null) return;

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
        }

        [RelayCommand]
        public async Task ToggleRouteAsync(TransportRouteEntity route)
        {
            if (route is null) return;

            CancellationToken token = new CancellationTokenSource().Token;
            await _favoriteService.ToggleFavoriteAsync(route, token);
        }

        [RelayCommand]
        public async Task ArrivalTimesVehicles(int stoppointId)
        {
            Models.GortransPerm.ArrivalTimesVehicles.ArrivalTimesVehicles result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _api.GetArrivalTimesVehiclesAsync(stoppointId);
            }, "Загрузка данных...");

            if (result is null)
            {
                await Shell.Current.DisplayPopupAsync("Данные отсутствуют.");
                return;
            }

            StoppointEntity stoppoint = await _dB.Stoppoints.FindAsync(stoppointId);

            Models.GortransPerm.ArrivalTimesVehicles.RouteType busRoute =
                result.RouteTypes.SingleOrDefault(rt => rt.RouteTypeId == (int)Models.GortransPerm.TypeOfRoute.Bus);

            if (busRoute is null || busRoute.Routes.Count == 0)
            {
                await Shell.Current.DisplayPopupAsync("Данные отсутствуют.");
                return;
            }

            StringBuilder popupMessage = new();

            popupMessage.Append(stoppoint is not null
                ? $"{stoppoint.StoppointName} ({stoppoint.Note})\n{busRoute.RouteTypeName}"
                : busRoute.RouteTypeName);

            foreach (var route in busRoute.Routes)
            {
                StringBuilder arrival = new();

                foreach (var vehicle in route.Vehicles)
                    arrival.Append($"{vehicle.ArrivalTime.SkipLast(3).ToString(0)} ({vehicle.ArrivalMinutes}м.) ");

                popupMessage.Append($"\n№{route.RouteNumber}: {arrival}");
            }

            await Shell.Current.DisplayPopupAsync(popupMessage.ToString());
        }

        private void FavoriteStoppointMessageHandler(object recipient, FavoriteStoppointChangedMessage message)
        {
            if (IsBusy) return;

            ArgumentNullException.ThrowIfNull(message);

            StoppointEntity sp = _dB.Stoppoints.FirstOrDefault(s => s.StoppointId == message.Value);

            if (sp is null) return;

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
