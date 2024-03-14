using CityTraffic.Models.Entities;
using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class TransportRoutesPage : ContentPage
{
	public TransportRoutesPage(TransportRoutesViewModel transportRoutesVM)
	{
		InitializeComponent();
		BindingContext = transportRoutesVM;
	}

    private void MenuFlyoutItem_Clicked(object sender, EventArgs e)
    {
		//if (BindingContext is TransportRoutesViewModel viewModel)
		//	if (sender is MenuFlyoutItem menu)
		//		viewModel.AddToFavoritesCommand.Execute(menu.CommandParameter);
    }
}