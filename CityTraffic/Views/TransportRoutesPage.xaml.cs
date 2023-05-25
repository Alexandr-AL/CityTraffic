using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class TransportRoutesPage : ContentPage
{
	public TransportRoutesPage(TransportRoutesViewModel transportRoutesVM)
	{
		InitializeComponent();
		BindingContext = transportRoutesVM;
	}
}