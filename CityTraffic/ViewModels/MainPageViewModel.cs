using CityTraffic.DAL;
using CityTraffic.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public MainPageViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            //_dB.Database.EnsureDeleted();
            _dB.Database.Migrate();
            //InitDb();
        }

        [RelayCommand]
        private async Task InitDb()
        {
            if (!_dB.TransportRoutes.Any())
            {
                _dB.TransportRoutes.AddRange(await GortransPermAPI.GetAllTransportRoutes());
                _dB.SaveChanges();
            }

            if (!_dB.Stoppoints.Any())
            {
                _dB.Stoppoints.AddRange(await GortransPermAPI.GetAllStoppoints(_dB.TransportRoutes.AsEnumerable()));
                _dB.SaveChanges();
            }
        }
    }
}