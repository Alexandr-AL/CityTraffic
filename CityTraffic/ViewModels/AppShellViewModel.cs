using CityTraffic.DAL;
using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Models.GortransPerm;
using CityTraffic.Models.GortransPerm.StoppointTimetable;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.ErrorHandler;
using CityTraffic.Services.ShowDataGortransService;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UraniumUI.Dialogs;

namespace CityTraffic.ViewModels
{
    public partial class AppShellViewModel : Base.ViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDataSyncService _dataSyncService;

        public AppShellViewModel(IErrorHandler errorHandler,
                                 IDialogService dialogService,
                                 IDataSyncService dataSyncService) : base(errorHandler)
        {
            _dialogService = dialogService;
            _dataSyncService = dataSyncService;
        }

        [RelayCommand]
        private async Task UpdateDatabase()
        {
            CancellationToken ct = new CancellationTokenSource().Token;
            (int count, int sec) result = default;

            await _errorHandler.SafeExecuteAsync(async () =>
            {
                result = await _dataSyncService.UpdateDatabaseAsync(ct);
            },"Обновление данных...");

            await _dialogService.DisplayPopupAsync($"Обновлено объектов: {result.count}\nза {result.sec} сек.");
        }
    }
}
