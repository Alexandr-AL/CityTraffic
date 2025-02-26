using CommunityToolkit.Maui.Views;

namespace CityTraffic.Extensions
{
    public static class ShellExtensions
    {
        private static Popup _popup;

        public static Task DisplayActivityIndicator(this Page page, string message, CancellationToken token = default)
        {
            _popup = new Popup
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
                        },
                        new ActivityIndicator
                        {
                            Color = Colors.OrangeRed,
                            IsRunning = true,
                            HorizontalOptions = LayoutOptions.Center
                        }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.White,
                    Padding = new Thickness(0, 35)
                },
                Color = Colors.Transparent,
                CanBeDismissedByTappingOutsideOfPopup = false
            };

            return Shell.Current.ShowPopupAsync(_popup, token);
        }

        public static Task HideActivityIndicator(this Page page, CancellationToken token = default)
        {
            if (Shell.Current.CurrentPage is null) return Task.CompletedTask;

            if (_popup is null) return Task.CompletedTask;

            return _popup?.CloseAsync(token: token);
            //return Shell.Current?.Navigation?.PopModalAsync();
        }
    }
}
