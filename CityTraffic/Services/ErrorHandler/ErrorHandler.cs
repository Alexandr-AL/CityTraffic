using CityTraffic.Infrastructure.GortransPermApi;
using CityTraffic.Services.DialogService;
using System.Net;

namespace CityTraffic.Services.ErrorHandler
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IDialogService _dialogService;

        public ErrorHandler(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task HandleErrorAsync(Exception ex)
        {
            string userMessage = GetUserMessage(ex);

            await _dialogService.ShowAlertAsync("Ошибка", userMessage);
        }

        public string GetUserMessage(Exception ex)
        {
            return ex switch
            {
                GortransPermApiException apiEx => apiEx.StatusCode switch
                {
                    HttpStatusCode.NotFound => $"{apiEx.Message}\nРесурс не найден ({(int)HttpStatusCode.NotFound})",
                    HttpStatusCode.BadRequest => $"{apiEx.Message}\nНекорректный запрос ({(int)HttpStatusCode.BadRequest})",
                    HttpStatusCode.RequestTimeout => $"{apiEx.Message}\nИстекло время ожидания ({(int)HttpStatusCode.RequestTimeout})",
                    _ => $"Ошибка получения данных ({(int)apiEx.StatusCode}-{apiEx.StatusCode})"
                },
                OperationCanceledException => $"Операция была отменена",

                _ => $"Произошла непредвиденная ошибка:\n{ex.Message}"
            };
        }
    }
}
