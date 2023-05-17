using CityTraffic.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CityTraffic.Migrator;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        using var host = builder
            .ConfigureServices(services => services
                .AddSqlite<CityTrafficDB>("Data source = CityTraffic.db", assembly => assembly
                    .MigrationsAssembly("CityTraffic")))
            .Build();
    }
}