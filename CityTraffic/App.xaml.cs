using CityTraffic.Extensions;
using CityTraffic.Services.ErrorHandler;

namespace CityTraffic;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		AppDomain.CurrentDomain.UnhandledException += OnGlobalException;
        TaskScheduler.UnobservedTaskException += OnTaskException;
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

    private void OnGlobalException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
            HandleException(ex);
    }

    private void OnTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        HandleException(e.Exception);
        e.SetObserved();
    }

    private void HandleException(Exception ex)
    {
        IErrorHandler errorHandler = IPlatformApplication.Current?.Services?.GetService<IErrorHandler>();

        errorHandler.HandleErrorAsync(ex);
    }
}
