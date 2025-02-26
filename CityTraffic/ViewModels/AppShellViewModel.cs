using CityTraffic.DAL;
using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Models.GortransPerm;
using CityTraffic.Models.GortransPerm.StoppointTimetable;
using CityTraffic.Services.DataSyncService;
using CityTraffic.Services.DialogService;
using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.Input;
using System.Text;

namespace CityTraffic.ViewModels
{
    public partial class AppShellViewModel : Base.ViewModel
    {
        private readonly GortransPermApi _gortransPermAPI;
        private readonly CityTrafficDB _dB;
        private readonly IDataSyncService _dataSyncService;

        public AppShellViewModel(GortransPermApi gortransPermAPI,
                                 CityTrafficDB dB,
                                 IErrorHandler errorHandler, 
                                 IDialogService dialogService,
                                 IDataSyncService dataSyncService) : base(errorHandler, dialogService)
        {
            _gortransPermAPI = gortransPermAPI;
            _dB = dB;
            _dataSyncService = dataSyncService;
        }

        [RelayCommand]
        private async Task UpdateDatabase()
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
            //await GetArrivalTimesVehicles(6101);
            //await GetStoppointTimetable(6101);
            //await GetTimeTableH("07", 6101);
            //await GetSearch("1");
            //await GetMovingAutos("07");
            //await GetNewsLinks();
            //await GetBoards();
        }

        private async Task GetBoards()
        {
            List<Models.GortransPerm.Board.Board> result = new();

            await SafeExecuteAsync(async () =>
            {
                result = (await _gortransPermAPI.GetBoardsAsync()).ToList();
            });

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            string popupMessage = string.Empty;

            foreach (var board in result)
            {
                popupMessage += $"({board.BoardId}){board.StopName}\nLat:{board.Lat} Lon:{board.Lon}\n\n";
            }
            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetNewsLinks()
        {
            Models.GortransPerm.News.News result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _gortransPermAPI.GetNewsLinksAsync();
            });

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            string popupMessage = string.Empty;

            foreach (var newsLink in result.NewsLinks)
            {
                popupMessage += $"{newsLink.ItemNumber} - {newsLink.PublishedDate}\n{newsLink.Title}\n{newsLink.Url}\n\n";
            }

            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetMovingAutos(string routeId)
        {
            Models.GortransPerm.MovingAutos.MovingAutos result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _gortransPermAPI.GetMovingAutosAsync(routeId);
            });

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            string popupMessage = string.Empty;

            foreach (var auto in result.Autos)
            {
                popupMessage += $"№{auto.RouteNumber}\n{auto.GosNom}\n{auto.T}\n";
            }

            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetSearch(string query)
        {
            Models.GortransPerm.Search.Search result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _gortransPermAPI.GetSearchAsync(query);
            });

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            string popupMessage = string.Empty;

            foreach (var searchResult in result.SearchResults)
            {
                popupMessage += $"{searchResult.ResultType}: {searchResult.ResultTitle} ";
                popupMessage += searchResult.Stoppoint is null 
                    ? "\n" 
                    : $"({searchResult.Stoppoint.Note})\n{searchResult.Stoppoint.Routes}\n";
            }

            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetTimeTableH(string routeId, int stoppointId)
        {
            Models.GortransPerm.TimeTableH.TimeTableH result = new();

            await SafeExecuteAsync(async () =>
            {
               result = await _gortransPermAPI.GetTimeTableHAsync(routeId, stoppointId);
            });

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            Models.Entities.StoppointEntity stoppoint = await _dB.Stoppoints.FindAsync(stoppointId);

            string popupMessage = $"{result.Date}\n{stoppoint.StoppointName} ({stoppoint.Note})\n№{result.Route.RouteNumber} ({result.Route.RouteName})";

            foreach (var timeTable in result.TimeTable)
            {
                foreach (var stopTime in timeTable.StopTimes)
                {
                    popupMessage += $"""

                                    {stopTime.ScheduledTime}
                                    """;
                }
            }
            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetStoppointTimetable(int stoppointId)
        {
            List<StoppointTimetable> result = new();

            await SafeExecuteAsync(async () =>
            {
                result = (await _gortransPermAPI.GetStoppointTimetableAsync(stoppointId)).ToList();
            }, "Загрузка данных...");

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            Models.Entities.StoppointEntity stoppoint = await _dB.Stoppoints.FindAsync(stoppointId);

            string popupMessage = $"{stoppoint.StoppointName} ({stoppoint.Note})";

            foreach (var item in result)
            {
                popupMessage += $"""

                                №{item.RouteNumber}: {item.RouteName} - {item.ScheduledTime}
                                """;
            }

            await _dialogService.ShowPopupAsync(popupMessage);
        }

        private async Task GetArrivalTimesVehicles(int stoppointId)
        {
            Models.GortransPerm.ArrivalTimesVehicles.ArrivalTimesVehicles result = new();

            await SafeExecuteAsync(async () =>
            {
                result = await _gortransPermAPI.GetArrivalTimesVehiclesAsync(stoppointId);
            }, "Загрузка данных...");

            if (result is null)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            Models.Entities.StoppointEntity stoppoint = await _dB.Stoppoints.FindAsync(stoppointId);

            Models.GortransPerm.ArrivalTimesVehicles.RouteType busRoute =
                result.RouteTypes.SingleOrDefault(rt => rt.RouteTypeId == (int)TypeOfRoute.Bus);

            if (busRoute is null || busRoute.Routes.Count == 0)
            {
                await _dialogService.ShowPopupAsync("Данные отсутствуют.");
                return;
            }

            StringBuilder popupMessage = new();

            popupMessage.Append(stoppoint is not null
                ? $"{stoppoint.StoppointName} ({stoppoint.Note})\n{busRoute.RouteTypeName}"
                : busRoute.RouteTypeName);

            foreach (var route in busRoute.Routes)
            {
                StringBuilder arrival = new();

                foreach (var vehicle in route.Vehicles)
                    arrival.Append($"{vehicle.ArrivalTime.SkipLast(3).ToString(0)} ({vehicle.ArrivalMinutes}м.) ");

                popupMessage.Append($"\n№{route.RouteNumber}: {arrival}");
            }

            await _dialogService.ShowPopupAsync(popupMessage.ToString());
        }
    }
}
