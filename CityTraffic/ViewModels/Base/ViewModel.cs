using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.ComponentModel;
using UraniumUI.Dialogs.Mopups;

namespace CityTraffic.ViewModels.Base
{
    public abstract partial class ViewModel: ObservableObject
    {
        private readonly IErrorHandler _errorHandler;

        protected ViewModel(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        protected async Task SafeExecuteAsync(Func<Task> action, string loadingMessage = null)
        {
            if (string.IsNullOrWhiteSpace(loadingMessage))
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    await _errorHandler.HandleErrorAsync(ex);
                }
            }
            else
            {
                using (await Shell.Current.DisplayProgressAsync("", loadingMessage))
                {
                    try
                    {
                        await action();
                    }
                    catch (Exception ex)
                    {
                        await _errorHandler.HandleErrorAsync(ex);
                    }
                }
            }

        }
    }
}
