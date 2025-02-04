using CityTraffic.DAL;
using CityTraffic.Services.DialogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CityTraffic.ViewModels
{

    public partial class MainPageViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;

        public MainPageViewModel(CityTrafficDB dB, IDialogService dialogService) : base(dialogService)
        {
            _dB = dB;
            //FavoritesTR = new(_dB.FavoritesTransportRoutes);
            //FavoritesSp = new(_dB.FavoritesStoppoints);
        }

        //[ObservableProperty]
        //private ObservableCollection<FavoritesTransportRoute> _favoritesTR;

        //[ObservableProperty]
        //private ObservableCollection<FavoritesStoppoint> _favoritesSp;

        

        //private void DeleteFavoritesTR(FavoritesTransportRoute favoritesTransportRoute)
        //{
        //    _dB.FavoritesTransportRoutes.Remove(favoritesTransportRoute);
        //    _dB.SaveChanges();
        //}
    }
}