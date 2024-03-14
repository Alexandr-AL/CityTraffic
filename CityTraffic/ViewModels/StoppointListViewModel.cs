using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CityTraffic.ViewModels
{
    public partial class StoppointListViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public StoppointListViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            _stoppoints = new(_dB.Stoppoints.OrderBy(x => x.StoppointName).AsEnumerable());
        }

        [ObservableProperty]
        private ObservableCollection<EntityStoppoint> _stoppoints;
    }
}
