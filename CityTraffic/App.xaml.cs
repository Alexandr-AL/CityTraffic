namespace CityTraffic;

public partial class App : Application
{
	public static readonly string APIKey = "982b9621-2fd5-4d65-95c2-f14eb2a241c4";

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window =  base.CreateWindow(activationState);
		window.Width = 600;
		window.Height = 800;
		return window;
    }
}
