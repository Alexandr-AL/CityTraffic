using CityTraffic.Services.DialogService;
using CityTraffic.Services.GortransPerm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;

namespace CityTraffic.ViewModels.Base
{
    public abstract partial class ViewModel: ObservableObject
    {
        protected readonly IDialogService _dialogService;

        protected ViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        [ObservableProperty]
        public partial bool IsBusy { get; set; } = false;

        protected async Task SafeExecuteAsync(Func<Task> action, string loadingMessage = null)
        {
            try
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(loadingMessage))
                {
                    await _dialogService.ShowLoadingAsync(loadingMessage);
                }
                await action();
            }
            catch (GortransPermException ex)
            {
                string message = ex.StatusCode switch
                {
                    HttpStatusCode.NotFound => $"Not found ({(int)HttpStatusCode.NotFound})",
                    HttpStatusCode.BadRequest =>$"Bad request ({(int)HttpStatusCode.BadRequest})",
                    _ => "Error receiving data"
                };

                await _dialogService.ShowAlertAsync("Error", message);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error",$"An unexpected error occurred:\n{ex.Message}");
            }
            finally
            {
                IsBusy = false;
                await _dialogService.HideLoadingAsync();
            }
        }
    }
}
