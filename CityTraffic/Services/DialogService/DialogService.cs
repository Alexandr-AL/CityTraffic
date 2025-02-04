using CityTraffic.Extensions;

namespace CityTraffic.Services.DialogService
{
    internal class DialogService : IDialogService
    {
        public async Task ShowAlertAsync(string title, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.DisplayAlert(title, message, "OK");
            });
        }

        public async Task ShowLoadingAsync(string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current?.CurrentPage?.DisplayActivityIndicator(message);
            });
        }

        public async Task HideLoadingAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current?.CurrentPage?.HideActivityIndicator();
            });
        }
    }
}
