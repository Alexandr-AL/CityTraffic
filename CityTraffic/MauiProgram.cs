using CityTraffic.DAL;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Services;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.ErrorHandler;
using CityTraffic.ViewModels;
using CityTraffic.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net;

namespace CityTraffic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<AppShell, AppShellViewModel>();
		builder.Services.AddSingleton<MainPage, MainPageViewModel>();
		builder.Services.AddSingleton<StoppointListPage, StoppointListViewModel>();
        builder.Services.AddSingleton<TransportRoutesPage, TransportRoutesViewModel>();

        builder.Services.AddSingleton<FavoritesPage>();

		builder.Services.AddSingleton<IDialogService, DialogService>();

		builder.Services.AddSingleton<IErrorHandler, ErrorHandler>();

        builder.Services.AddSingleton<IDataSyncService, DataSyncService>();

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
