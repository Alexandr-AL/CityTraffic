using CityTraffic.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.DAL
{
    public class CityTrafficDB : DbContext
    {
        public CityTrafficDB(DbContextOptions<CityTrafficDB> options) : base(options) { }

        public DbSet<TransportRoute> TransportRoutes {get;set;}
        public DbSet<Stoppoint> Stoppoints { get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransportRoute>()
                .HasKey(transportRoute => transportRoute.Id);

            modelBuilder.Entity<Stoppoint>()
                .HasKey(stoppoint => stoppoint.Id);
        }
    }
}
