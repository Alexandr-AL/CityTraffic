
namespace CityTraffic.Services.FavoriteService
{
    public interface IFavoriteService
    {
        Task ToggleFavoriteAsync<T>(T favoriteItem, CancellationToken token = default) where T : class;
    }
}