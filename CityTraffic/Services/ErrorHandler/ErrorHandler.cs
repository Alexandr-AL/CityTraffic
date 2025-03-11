using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using System.Net;

namespace CityTraffic.Services.ErrorHandler
{
    public class ErrorHandler : IErrorHandler
    {

        public ErrorHandler()
        {
        }

        public async Task HandleErrorAsync(Exception ex)
        {
            string userMessage = GetUserMessage(ex);

            await Shell.Current.DisplayPopupAsync($"Error\n{userMessage}");
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
