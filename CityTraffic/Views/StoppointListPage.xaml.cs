using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class StoppointListPage : ContentPage
{
	public StoppointListPage(StoppointListViewModel stoppointListVM)
	{
		InitializeComponent();
		BindingContext = stoppointListVM;
	}
}