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
    public partial class TransportRoutesViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly IFavoriteService _favoriteService;

        public TransportRoutesViewModel(CityTrafficDB dB, 
                                        IErrorHandler errorHandler,
                                        IFavoriteService favoriteService) : base(errorHandler)
        {
            _dB = dB;
            _favoriteService = favoriteService;

            LoadRoutes();

            WeakReferenceMessenger.Default.Register<DataSyncServiceChangedMessage>(this, DataSyncServiceMessageHandler);
        }

        [ObservableProperty]
        public partial ObservableCollection<TransportRouteEntity> TransportRoutes { get; set; }

        [RelayCommand]
        public async Task ToggleFavorite(TransportRouteEntity route)
        {
            await _favoriteService.ToggleFavoriteAsync(route);
        }

        [RelayCommand]
        public void LoadRoutes()
        {
            TransportRoutes = _dB.TransportRoutes.OrderBy(t => t.RouteId).Include(tr => tr.Stoppoints).ToObservableCollection();
        }

        private void DataSyncServiceMessageHandler(object recipient, DataSyncServiceChangedMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.Value == 0) return;

            LoadRoutes();
        }
    }
}
