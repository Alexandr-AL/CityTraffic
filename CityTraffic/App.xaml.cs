namespace CityTraffic;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new AppShell();
		MainPage = IPlatformApplication.Current.Services.GetService<AppShell>();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window =  base.CreateWindow(activationState);
		window.Width = 600;
		window.Height = 800;
		return window;
    }
}
