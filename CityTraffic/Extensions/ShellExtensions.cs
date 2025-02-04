using CommunityToolkit.Maui.Views;

namespace CityTraffic.Extensions
{
    public static class ShellExtensions
    {
        public static Task DisplayActivityIndicator(this Page page, string message)
        {
            ActivityIndicator activityIndicator = new ActivityIndicator();

            Label label = new Label() 
            {
                Text = message,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center
            };

            VerticalStackLayout stackLayout = new VerticalStackLayout()
            {
                Children = { activityIndicator, label },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.White,
                Padding = 20,
                Spacing = 10
            };

            Popup popup = new Popup()
            {
                Content = stackLayout,
                Color = Colors.Transparent,
                CanBeDismissedByTappingOutsideOfPopup = false
            };

            return Shell.Current.ShowPopupAsync(popup);
        }

        public static Task HideActivityIndicator(this Page page)
        {
            if (Shell.Current.CurrentPage is null) return Task.CompletedTask;

            return Shell.Current?.Navigation?.PopModalAsync();
        }
    }
}
