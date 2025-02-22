using CityTraffic.DAL;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.Input;

namespace CityTraffic.ViewModels
{
    public partial class AppShellViewModel : Base.ViewModel
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermApi _gortransPermAPI;
        private readonly IDataSyncService _dataSyncService;

        public AppShellViewModel(CityTrafficDB dB, 
                                 GortransPermApi gortransPermAPI, 
                                 IErrorHandler errorHandler, 
                                 IDialogService dialogService,
                                 IDataSyncService dataSyncService) : base(errorHandler, dialogService)
        {
            _dB = dB;
            _gortransPermAPI = gortransPermAPI;
            _dataSyncService = dataSyncService;
            //_ = Init();
        }

        [RelayCommand]
        private async Task Init()
        {
            CancellationToken ct = new CancellationTokenSource().Token;
            (int count, int sec) result = default;

            await SafeExecuteAsync(async () =>
            {
                result = await _dataSyncService.InitializeDatabaseAsync(ct);
            },"Инициализация БД...");

            await _dialogService.ShowPopupAsync($"Добавлено новых объектов: {result.count}\nза {result.sec} сек.");
        }

        [RelayCommand]
        private async Task Update()
        {
            CancellationToken ct = new CancellationTokenSource().Token;
            (int count, int sec) result = default;

            await SafeExecuteAsync(async () =>
            {
                result = await _dataSyncService.UpdateDatabaseAsync(ct);
            },"Обновление данных...");

            await _dialogService.ShowPopupAsync($"Обновлено объектов: {result.count}\nза {result.sec} сек.");
        }

        [RelayCommand]
        private async Task TestAPI()
        {
            Models.GortransPerm.ArrivalTimesVehicles.ArrivalTimesVehicles result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _gortransPermAPI.GetArrivalTimesVehiclesAsync(6101);
            },"Загрузка данных...");

            if (result.RouteTypes.Count > 0)
                await _dialogService.ShowPopupAsync($"""
                    {result.RouteTypes[0].RouteTypeName}
                    Маршрут № {result.RouteTypes[0].Routes[0].RouteNumber}
                    Время прибытия: {result.RouteTypes[0].Routes[0].Vehicles[0].ArrivalTime}
                    Прибытие через: {result.RouteTypes[0].Routes[0].Vehicles[0].ArrivalMinutes} мин.
                    """);
            else await _dialogService.ShowPopupAsync("Данные отсутствуют.");
        }
    }
}
