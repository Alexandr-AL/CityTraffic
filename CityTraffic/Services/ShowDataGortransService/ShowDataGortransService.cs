using CityTraffic.DAL;
using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UraniumUI.Dialogs;

namespace CityTraffic.Services.ShowDataGortransService
{
    public class ShowDataGortransService : IShowDataGortransService
    {
        private readonly CityTrafficDB _dB;
        private readonly GortransPermApi _api;
        private readonly IDialogService _dialogService;

        public ShowDataGortransService(CityTrafficDB cityTrafficDB,
                                       GortransPermApi gortransPermApi,
                                       IDialogService dialogService)
        {
            _dB = cityTrafficDB;
            _api = gortransPermApi;
            _dialogService = dialogService;
        }

        private async Task ShowMessageAsync<T>(T message, CancellationToken token = default) where T : class
        {
            switch (message)
            {
                case string msg:
                    await _dialogService.DisplayPopupAsync(msg, token);
                    break;

                case StringBuilder msg:
                    string usrMsg = msg.ToString();
                    ArgumentException.ThrowIfNullOrEmpty(usrMsg);
                    await _dialogService.DisplayPopupAsync(usrMsg, token);
                    break;
                default: throw new ArgumentException($"Неверный тип аргумента: {typeof(T)}");
            }
        }

        public async Task ShowArrivalTimesVehicles(StoppointEntity stoppoint, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(stoppoint);

            Models.GortransPerm.ArrivalTimesVehicles.ArrivalTimesVehicles result =
                await _api.GetArrivalTimesVehiclesAsync(stoppoint.StoppointId, token);

            ArgumentNullException.ThrowIfNull(result);

            if (result.RouteTypes is null || result.RouteTypes.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            userMessage.AppendLine($"{stoppoint.StoppointName} ({stoppoint.Note})");

            foreach (var routeType in result.RouteTypes)
            {
                userMessage.AppendLine($"{routeType.RouteTypeName}:");

                foreach (var route in routeType.Routes)
                {
                    StringBuilder arrival = new();

                    foreach (var vehicle in route.Vehicles)
                    {
                        arrival.Append($"{vehicle.ArrivalTime.SkipLast(3).ToString(0)} ({vehicle.ArrivalMinutes}м.) ");
                    }

                    userMessage.AppendLine($"№{route.RouteNumber}: {arrival}");
                }
            }
            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowStoppointTimetable(StoppointEntity stoppoint, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(stoppoint);

            List<Models.GortransPerm.StoppointTimetable.StoppointTimetable> result =
                (await _api.GetStoppointTimetableAsync(stoppoint.StoppointId, token: token)).ToList();

            if (result is null || result.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            var groupStoppointTimetable = result.GroupBy(s => (s.RouteNumber, s.RouteName));

            StringBuilder userMessage = new();

            userMessage.AppendLine($"{stoppoint.StoppointName} ({stoppoint.Note})");

            foreach (var item in groupStoppointTimetable)
            {
                userMessage.AppendLine($"№{item.Key.RouteNumber} {item.Key.RouteName}");

                foreach (var subitem in item)
                {
                    userMessage.Append($"{subitem.ScheduledTime.SkipLast(3).ToString(0)} ");
                }

                userMessage.AppendLine();
            }

            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowTimeTableH(string routeId, int stoppointId, CancellationToken token = default)
        {
            Models.GortransPerm.TimeTableH.TimeTableH result =
                await _api.GetTimeTableHAsync(routeId, stoppointId, token: token);

            ArgumentNullException.ThrowIfNull(result);

            if (result.TimeTable is null || result.TimeTable.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StoppointEntity stoppoint = await _dB.Stoppoints.AsNoTracking().FirstOrDefaultAsync(s => s.StoppointId == stoppointId, token);

            if (stoppoint is null)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            userMessage.AppendLine(result.Date);
            userMessage.AppendLine($"{stoppoint.StoppointName} ({stoppoint.Note})");
            userMessage.AppendLine($"№{result.Route.RouteNumber} ({result.Route.RouteName})");

            foreach (var timeTable in result.TimeTable)
            {
                foreach (var stopTime in timeTable.StopTimes)
                {
                    userMessage.Append($"{stopTime.ScheduledTime.SkipLast(3).ToString(0)} ");
                }
            }
            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowSearch(string query, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            Models.GortransPerm.Search.Search result = await _api.GetSearchAsync(query, token);

            ArgumentNullException.ThrowIfNull(result);

            if (result.SearchResults is null || result.SearchResults.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            foreach (var searchResult in result.SearchResults)
            {
                userMessage.AppendLine($"{searchResult.ResultType}: {searchResult.ResultTitle}");

                if (searchResult.Stoppoint is not null)
                    userMessage.AppendLine($" {searchResult.Stoppoint.Note} {searchResult.Stoppoint.Routes}");
            }

            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowMovingAutos(string routeId, CancellationToken token = default)
        {
            Models.GortransPerm.MovingAutos.MovingAutos result = await _api.GetMovingAutosAsync(routeId, token);

            ArgumentNullException.ThrowIfNull(result);

            if (result.Autos is null || result.Autos.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            foreach (var auto in result.Autos)
            {
                userMessage.AppendLine(auto.RouteNumber);
                userMessage.AppendLine(auto.GosNom);
                userMessage.AppendLine(auto.T);
                userMessage.AppendLine();
            }

            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowNewsLinks(CancellationToken token = default)
        {
            Models.GortransPerm.News.News result = await _api.GetNewsLinksAsync(token);

            ArgumentNullException.ThrowIfNull(result);

            if (result.NewsLinks is null || result.NewsLinks.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            foreach (var newsLink in result.NewsLinks)
            {
                userMessage.AppendLine($"{newsLink.ItemNumber} - {newsLink.PublishedDate}");
                userMessage.AppendLine(newsLink.Title);
                userMessage.AppendLine(newsLink.Url);
                userMessage.AppendLine();
            }

            await ShowMessageAsync(userMessage.ToString(), token);
        }

        public async Task ShowBoards(CancellationToken token = default)
        {
            List<Models.GortransPerm.Board.Board> result = (await _api.GetBoardsAsync(token)).ToList();

            if (result is null || result.Count == 0)
            {
                await _dialogService.DisplayPopupAsync("Данные отсутствуют.", token);
                return;
            }

            StringBuilder userMessage = new();

            foreach (var board in result)
            {
                userMessage.AppendLine($"{board.StopName}");
                userMessage.AppendLine($"Lat:{board.Lat} Lon:{board.Lon}");
                userMessage.AppendLine();
            }
            await ShowMessageAsync(userMessage.ToString(), token);
        }
    }
}
