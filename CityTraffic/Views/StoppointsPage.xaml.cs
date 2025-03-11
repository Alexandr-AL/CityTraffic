using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class StoppointsPage : ContentPage
{
	public StoppointsPage(StoppointsViewModel stoppointListVM)
	{
		InitializeComponent();
		BindingContext = stoppointListVM;
	}
}