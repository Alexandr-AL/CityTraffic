
namespace CityTraffic.Services.DataSyncService
{
    public interface IDataSyncService
    {
        Task<(int, int)> InitializeDatabaseAsync(CancellationToken token = default);

        Task<(int, int)> UpdateDatabaseAsync(CancellationToken token = default);
    }
}