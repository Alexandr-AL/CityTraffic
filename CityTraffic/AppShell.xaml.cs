using CityTraffic.ViewModels;

namespace CityTraffic;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel appShellVM)
	{
		InitializeComponent();
        BindingContext = appShellVM;
    }
}
