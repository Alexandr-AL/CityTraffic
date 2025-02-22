namespace CityTraffic.Services.ErrorHandler
{
    public interface IErrorHandler
    {
        Task HandleErrorAsync(Exception ex);

        string GetUserMessage(Exception ex);
    }
}
