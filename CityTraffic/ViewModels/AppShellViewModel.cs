using CityTraffic.DAL;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.GortransPerm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CityTraffic.ViewModels
{
    public partial class AppShellViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermAPI _gortransPermAPI;

        public AppShellViewModel(CityTrafficDB dB, GortransPermAPI gortransPermAPI, IDialogService dialogService) : base(dialogService)
        {
            _dB = dB;
            _gortransPermAPI = gortransPermAPI;
            //_ = Init();
        }

        [RelayCommand]
        private async Task UpdateDb()
        {
            var result = await _dB.UpdateDB();

            await Shell.Current.DisplayAlert($"Updated {result.Item1} entities in {result.Item2} sec.",
                                             $"Count Transport routes {_dB.TransportRoutes.Count()}\n" +
                                             $"Count Stoppoints {_dB.Stoppoints.Count()}", "OK");
        }

        private async Task Init()
        {
            var result = await _dB.InitDB();

            if (result > 0)
                await Shell.Current.DisplayAlert("", $"DB Initialized\nEntities added:{result}", "OK");
        }

        [RelayCommand]
        private async Task TestAPI()
        {
            try
            {
                var result = await _gortransPermAPI.GetArrivalTimesVehicles((50200).ToString());

                await Shell.Current.DisplayAlert(result.RouteTypes[0].Routes[0].RouteId,
                                                 result.RouteTypes[0].Routes[0].Vehicles[0].ArrivalTime,
                                                 "OK");
            }
            catch (GortransPermException ex)
            {
                await Shell.Current.DisplayAlert(ex.StatusCode.ToString(), ex.UserMessage(), "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
