namespace CityTraffic;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
		Window window = new Window(IPlatformApplication.Current?.Services.GetService<AppShell>());
		DisplayInfo displayInfo = DeviceDisplay.Current.MainDisplayInfo;

		window.Width = 600;
		window.Height = 800;
		window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
        window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;

        return window;
    }
}
