using CityTraffic.DAL;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.ErrorHandler;
using CityTraffic.Services.FavoriteService;
using CityTraffic.ViewModels;
using CityTraffic.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Polly;
using System.Net;
using UraniumUI;

namespace CityTraffic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.UseUraniumUI()
			.UseUraniumUIMaterial()
			.ConfigureMopups()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddMaterialSymbolsFonts();
			});

		builder.Services.AddSingleton<AppShell, AppShellViewModel>();
		builder.Services.AddSingleton<MainPage, MainPageViewModel>();
		builder.Services.AddSingleton<StoppointsPage, StoppointsViewModel>();
        builder.Services.AddSingleton<TransportRoutesPage, TransportRoutesViewModel>();
        builder.Services.AddSingleton<FavoriteTransportRoutesPage, FavoriteTransportRoutesViewModel>();
        builder.Services.AddSingleton<FavoriteStoppointsPage, FavoriteStoppointsViewModel>();

        builder.Services.AddSingleton<IErrorHandler, ErrorHandler>();

        builder.Services.AddSingleton<IDataSyncService, DataSyncService>();

		builder.Services.AddSingleton<IFavoriteService, FavoriteService>();

		builder.Services.AddMopupsDialogs();

        builder.Services.AddHttpClient<GortransPermApi>()
						.AddResilienceHandler("GtpApiHandler", b =>
						{
							b.AddRetry(new HttpRetryStrategyOptions()
							{
								MaxRetryAttempts = 3,
								Delay = TimeSpan.FromSeconds(3),
								ShouldHandle = args => ValueTask.FromResult(
									args.Outcome.Exception is GortransPermApiException { StatusCode: HttpStatusCode.RequestTimeout}),
							})
							.AddTimeout(TimeSpan.FromSeconds(10));
						});

		string connectionString = $"Data Source = {GetDbPath()}CityTraffic.db";
		builder.Services.AddSqlite<CityTrafficDB>(connectionString, 
												  optionsAction: optAct => optAct.EnableSensitiveDataLogging());

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
