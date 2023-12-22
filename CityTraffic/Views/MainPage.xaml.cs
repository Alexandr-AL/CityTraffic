using CityTraffic.DAL;
using CityTraffic.ViewModels;

namespace CityTraffic.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageVM)
	{
		InitializeComponent();
		BindingContext = mainPageVM;
	}

    private void MenuFlyoutItem_Clicked(object sender, EventArgs e)
    {

    }
}