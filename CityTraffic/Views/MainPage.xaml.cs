using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageVM)
	{
		InitializeComponent();
		BindingContext = mainPageVM;
	}
}