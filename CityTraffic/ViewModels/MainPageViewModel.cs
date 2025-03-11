using CityTraffic.Services.ErrorHandler;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        public MainPageViewModel(IErrorHandler errorHandler) : base(errorHandler)
        {
        }
    }
}