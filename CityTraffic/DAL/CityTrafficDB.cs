using CityTraffic.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.DAL
{
    public class CityTrafficDB : DbContext
    {
        public CityTrafficDB(DbContextOptions<CityTrafficDB> options) : base(options) { }

        public DbSet<TransportRouteEntity> TransportRoutes { get; set; }
        public DbSet<StoppointEntity> Stoppoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransportRouteEntity>(e =>
            {
                e.HasKey(tr => tr.RouteId)
                 .HasName("PK_TransportRoutes");

                e.Property(tr => tr.RouteId)
                 .ValueGeneratedNever();

                e.HasMany(tr => tr.Stoppoints)
                 .WithMany(s => s.Routes);
            });

            modelBuilder.Entity<StoppointEntity>(e =>
            {
                e.HasKey(s => s.StoppointId)
                 .HasName("PK_Stoppoints");

                e.Property(s => s.StoppointId)
                 .ValueGeneratedNever();

                e.Property(s => s.Location)
                 .HasColumnName("location")
                 .HasConversion(loc => $"{loc.Latitude} {loc.Longitude}", 
                                loc => new Models.Entities.Location(loc));

                e.HasMany(s => s.Routes)
                 .WithMany(tr => tr.Stoppoints);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
