namespace CityTraffic.Services.DialogService
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message);

        Task ShowPopupAsync(string message);

        Task ShowLoadingAsync(string message);

        Task HideLoadingAsync();
    }
}
