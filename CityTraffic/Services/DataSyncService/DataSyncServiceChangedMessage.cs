using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CityTraffic.Services.DataSyncService
{
    class DataSyncServiceChangedMessage : ValueChangedMessage<int>
    {
        public DataSyncServiceChangedMessage(int countUpdated) : base(countUpdated)
        {
        }
    }
}
