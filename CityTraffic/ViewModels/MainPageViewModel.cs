using CityTraffic.DAL;
using CityTraffic.Models;
using CityTraffic.Models.Interfaces;
using CityTraffic.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly HttpClient _httpClient;

        public MainPageViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            _httpClient = new();

            InitDb();
        }

        private void InitDb()
        {
            _dB?.Database.Migrate();
        }

        [RelayCommand]
        private async Task<List<IStoppoint>> GetListAllStops()
        {
            var allBusAndTramStops = new List<IStoppoint>();

            var allTransport = await GortransPermAPI.GetRouteTypes(_httpClient);

            if (allTransport is null) return default;

            var allRouteIdInTramAndBus = allTransport
                //.Where(typeTransport => typeTransport.RouteTypeId is (((int)RouteType.Bus) or (int)RouteType.Tram))
                .SelectMany(transportType => transportType.Children
                    .Select(transport => transport.RouteId))
                .ToList();
            
            foreach (var routeId in allRouteIdInTramAndBus)
            {
                var routeInfo = await GortransPermAPI.GetFullRoute(routeId, _httpClient);

                if (routeInfo is not null)
                {
                    allBusAndTramStops.AddRange(routeInfo.FwdStoppoints
                        .Where(stoppoint =>  !allBusAndTramStops
                            .Select(sp => sp.StoppointId)
                            .Contains(stoppoint.StoppointId)));

                    allBusAndTramStops.AddRange(routeInfo.BkwdStoppoints
                        .Where(stoppoint => !allBusAndTramStops
                            .Select(sp => sp.StoppointId)
                            .Contains(stoppoint.StoppointId)));
                }
            }
            return allBusAndTramStops;
        }
    }
}