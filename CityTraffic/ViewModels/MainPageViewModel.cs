using CityTraffic.DAL;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public MainPageViewModel(CityTrafficDB dB, IErrorHandler errorHandler, IDialogService dialogService) : base(errorHandler, dialogService)
        {
            _dB = dB;
        }

    }
}