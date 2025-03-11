
namespace CityTraffic.Services.DataSyncService
{
    public interface IDataSyncService
    {
        Task<(int, int)> UpdateDatabaseAsync(CancellationToken token = default);
    }
}