using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CityTraffic.ViewModels
{
    public partial class TransportRoutesViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public TransportRoutesViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            _transportRoutes = new(_dB.TransportRoutes.OrderBy(x => x.RouteId).AsEnumerable());
        }

        [ObservableProperty]
        private ObservableCollection<TransportRoute> _transportRoutes;
                
        [RelayCommand]
        private void AddToFavorites(TransportRoute transportRoute)
        {
            var tr = _dB.TransportRoutes.Include(tr=> tr.FavoritesTransportRoute).SingleOrDefault(e => e.Id == transportRoute.Id);

            tr.FavoritesTransportRoute = new()
            {
                TransportRouteId = tr.Id,
                TransportRoute = tr
            };
            _dB.SaveChanges();
        }
    }
}
