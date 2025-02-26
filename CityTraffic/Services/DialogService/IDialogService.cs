namespace CityTraffic.Services.DialogService
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message);

        Task ShowPopupAsync(string message, CancellationToken token = default);

        Task ShowLoadingAsync(string message, CancellationToken token = default);

        Task HideLoadingAsync(CancellationToken token = default);
    }
}
