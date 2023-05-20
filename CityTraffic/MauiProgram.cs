using CityTraffic.DAL;
using CityTraffic.Services;
using CityTraffic.ViewModels;
using CityTraffic.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityTraffic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();

        string connectionString = $"Data Source = {GetDbPath()}CityTraffic.db";
		builder.Services.AddSqlite<CityTrafficDB>(connectionString);

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    private static string GetDbPath() =>
        DeviceInfo.Platform == DevicePlatform.WinUI
        ? AppDomain.CurrentDomain.BaseDirectory
        : FileSystem.AppDataDirectory + "\\";
}
