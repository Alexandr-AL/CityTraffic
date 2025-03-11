using CommunityToolkit.Maui.Views;

namespace CityTraffic.Extensions
{
    public static class ShellExtensions
    {
        public static async Task DisplayPopupAsync(this Page page, string message, CancellationToken token = default)
        {
            Popup _popup = new Popup
            {
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = message,
                            FontSize = 14,
                            HorizontalOptions = LayoutOptions.Center,
                            BackgroundColor = Colors.LightCyan
                        }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.White
                },
                Color = Colors.Transparent,
                CanBeDismissedByTappingOutsideOfPopup = true
            };

            await Shell.Current.ShowPopupAsync(_popup, token);
        }

        public static async Task DisplayPopupAsync(this UraniumUI.Dialogs.IDialogService dialogService, string message, CancellationToken token = default)
        {
            Popup _popup = new Popup
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

            await Shell.Current.ShowPopupAsync(_popup, token);
        }
    }
}
