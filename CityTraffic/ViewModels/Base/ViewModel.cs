using CityTraffic.Services.DialogService;
using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CityTraffic.ViewModels.Base
{
    public abstract partial class ViewModel: ObservableObject
    {
        private readonly IErrorHandler _errorHandler;
        protected readonly IDialogService _dialogService;

        protected ViewModel(IErrorHandler errorHandler, IDialogService dialogService)
        {
            _errorHandler = errorHandler;
            _dialogService = dialogService;
        }

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        protected async Task SafeExecuteAsync(Func<Task> action, string loadingMessage = null)
        {
            try
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(loadingMessage))
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await _dialogService.ShowLoadingAsync(loadingMessage);
                    });
                }

                await action();
            }
            catch (Exception ex)
            {
                await _errorHandler.HandleErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
                await _dialogService.HideLoadingAsync();
            }
        }
    }
}
