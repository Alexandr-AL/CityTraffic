namespace CityTraffic.Services.ErrorHandler
{
    public interface IErrorHandler
    {
        Task HandleErrorAsync(Exception ex);

        Task SafeExecuteAsync(Func<Task> action, string loadingMessage = null);
    }
}
