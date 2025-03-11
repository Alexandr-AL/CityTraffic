using CityTraffic.Models.Entities;
using CityTraffic.ViewModels;
using UraniumUI.Pages;

namespace CityTraffic.Views;

public partial class FavoriteTransportRoutesPage : UraniumContentPage
{
	public FavoriteTransportRoutesPage(FavoriteTransportRoutesViewModel favoriteTransportRoutesViewModel)
	{
		InitializeComponent();
		BindingContext = favoriteTransportRoutesViewModel;
	}
}