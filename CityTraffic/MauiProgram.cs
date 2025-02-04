using CityTraffic.DAL;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.GortransPerm;
using CityTraffic.ViewModels;
using CityTraffic.Views;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

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

		builder.Services.AddHttpClient<GortransPermAPI>();

		builder.Services.AddSingleton<IDialogService, DialogService>();

		string connectionString = $"Data Source = {GetDbPath()}CityTraffic.db";
		builder.Services.AddSqlite<CityTrafficDB>(connectionString);

		builder.Services.AddResiliencePipeline("retry-pipeline", pollyBuilder =>
		{
			pollyBuilder.AddRetry(new Polly.Retry.RetryStrategyOptions()
			{
				MaxRetryAttempts = 3,
				Delay = TimeSpan.FromSeconds(2),
				ShouldHandle = args => ValueTask.FromResult(
					args.Outcome.Exception is GortransPermException { StatusCode: System.Net.HttpStatusCode.RequestTimeout })
			});
		});

		
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
