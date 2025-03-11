using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CityTraffic.Services.FavoriteService
{
    public class FavoriteStoppointChangedMessage : ValueChangedMessage<int>
    {
        public FavoriteStoppointChangedMessage(int stoppointId) : base(stoppointId) { }
    }
}
