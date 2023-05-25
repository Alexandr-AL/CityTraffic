using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
