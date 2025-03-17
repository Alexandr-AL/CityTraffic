using CityTraffic.Extensions;
using CityTraffic.Infrastructure.GortransPermApi;
using System.Net;
using UraniumUI.Dialogs;
using UraniumUI.Dialogs.Mopups;

namespace CityTraffic.Services.ErrorHandler
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IDialogService _dialogService;

        public ErrorHandler(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public virtual async Task HandleErrorAsync(Exception ex)
        {
            string userMessage = GetUserMessage(ex);

            await _dialogService.DisplayPopupAsync($"Error\n{userMessage}");
        }

        private string GetUserMessage(Exception ex)
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

        public virtual async Task SafeExecuteAsync(Func<Task> action, string loadingMessage = null)
        {
            if (string.IsNullOrWhiteSpace(loadingMessage))
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(ex);
                }
            }
            else
            {
                using (await _dialogService.DisplayProgressAsync("", loadingMessage))
                {
                    try
                    {
                        await action();
                    }
                    catch (Exception ex)
                    {
                        await HandleErrorAsync(ex);
                    }
                }
            }
        }
    }
}
