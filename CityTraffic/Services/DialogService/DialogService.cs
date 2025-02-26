using CityTraffic.Extensions;
using CityTraffic.Views;
using CommunityToolkit.Maui.Views;

namespace CityTraffic.Services.DialogService
{
    public class DialogService : IDialogService
    {
        public async Task ShowAlertAsync(string title, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.DisplayAlert(title, message, "OK");
            });
        }

        public async Task ShowPopupAsync(string message, CancellationToken token = default)
        {
            Popup popup = new Popup
            {
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = message,
                            FontSize = 14,
                            HorizontalOptions = LayoutOptions.Center
                        }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.White
                },
                Color = Colors.Transparent,
                CanBeDismissedByTappingOutsideOfPopup = true
            };

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                return await Shell.Current.ShowPopupAsync(popup, token);
            });
        }

        public async Task ShowLoadingAsync(string message, CancellationToken token = default)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current?.CurrentPage?.DisplayActivityIndicator(message, token);
            });
        }

        public async Task HideLoadingAsync(CancellationToken token = default)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current?.CurrentPage?.HideActivityIndicator(token);
            });
        }
    }
}
