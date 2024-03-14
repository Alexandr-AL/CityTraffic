using CityTraffic.DAL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CityTraffic.ViewModels
{
    public partial class AppShellViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public AppShellViewModel(CityTrafficDB dB)
        {
            _dB = dB;
            _ = Init();
        }

        [ObservableProperty]
        private bool _indicator = false;

        [RelayCommand]
        private async Task UpdateDb()
        {
            Indicator = true;

            var result = await _dB.UpdateDB();

            await Shell.Current.DisplayAlert($"Updated {result.Item1} entities in {result.Item2} sec.",
                                             $"Count Transport routes {_dB.TransportRoutes.Count()}\n" +
                                             $"Count Stoppoints {_dB.Stoppoints.Count()}", "OK");
            Indicator = false;
        }

        private async Task Init()
        {
            var result = await _dB.InitDB();

            if (result > 0)
                await Shell.Current.DisplayAlert("", $"DB Initialized\nEntities added:{result}", "OK");
        }
    }
}
