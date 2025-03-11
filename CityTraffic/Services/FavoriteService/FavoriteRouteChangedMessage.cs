using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CityTraffic.Services.FavoriteService
{
    public class FavoriteRouteChangedMessage : ValueChangedMessage<string>
    {
        public FavoriteRouteChangedMessage(string routeId) : base(routeId) { }
    }
}
