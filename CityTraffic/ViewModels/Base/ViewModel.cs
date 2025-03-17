using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.ComponentModel;
using UraniumUI.Dialogs.Mopups;

namespace CityTraffic.ViewModels.Base
{
    public abstract partial class ViewModel: ObservableObject
    {
        protected readonly IErrorHandler _errorHandler;

        protected ViewModel(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        [ObservableProperty]
        public partial bool IsBusy { get; set; }
    }
}
