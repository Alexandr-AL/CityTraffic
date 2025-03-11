using CityTraffic.DAL;
using CityTraffic.Models.Entities;
using CityTraffic.Services.ErrorHandler;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.Services.FavoriteService
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IErrorHandler _errorHandler;
        private readonly CityTrafficDB _dB;

        public FavoriteService(IErrorHandler errorHandler, CityTrafficDB cityTrafficDB)
        {
            _errorHandler = errorHandler;
            _dB = cityTrafficDB;
        }

        public async Task ToggleFavoriteAsync<T>(T favoriteItem, CancellationToken token = default) where T : class
        {
            try
            {
                switch (favoriteItem)
                {
                    case TransportRouteEntity tr:
                        await ToggleFavoriteTransportRouteAsync(tr, token);
                        break;

                    case StoppointEntity st:
                        await ToggleFavoriteStoppointAsync(st, token);
                        break;

                    default: throw new ArgumentException($"Неверный тип аргумента: {nameof(favoriteItem)}");
                }
            }
            catch (Exception ex)
            {
                await _errorHandler.HandleErrorAsync(ex);
            }
        }

        private async Task ToggleFavoriteTransportRouteAsync(TransportRouteEntity transportRoute, CancellationToken token = default)
        {
            if (transportRoute is null) return;

            TransportRouteEntity tr = await _dB.TransportRoutes.FirstOrDefaultAsync(t => t.RouteId == transportRoute.RouteId, token);

            if (tr is null) return;

            tr.IsFavorite = !tr.IsFavorite;

            await _dB.SaveChangesAsync(token);

            WeakReferenceMessenger.Default.Send(new FavoriteRouteChangedMessage(transportRoute.RouteId));
        }

        private async Task ToggleFavoriteStoppointAsync(StoppointEntity stoppoint, CancellationToken token = default)
        {
            if (stoppoint is null) return;

            StoppointEntity sp = await _dB.Stoppoints.FirstOrDefaultAsync(s => s.StoppointId == stoppoint.StoppointId, token);

            if (sp is null) return;

            sp.IsFavorite = !sp.IsFavorite;

            await _dB.SaveChangesAsync(token);

            WeakReferenceMessenger.Default.Send(new FavoriteStoppointChangedMessage(stoppoint.StoppointId));
        }
    }
}
