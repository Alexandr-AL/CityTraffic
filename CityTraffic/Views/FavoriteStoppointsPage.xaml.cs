using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class FavoriteStoppointsPage : ContentPage
{
	public FavoriteStoppointsPage(FavoriteStoppointsViewModel favoriteStoppointsViewModel)
	{
		InitializeComponent();
		BindingContext = favoriteStoppointsViewModel;
	}
}