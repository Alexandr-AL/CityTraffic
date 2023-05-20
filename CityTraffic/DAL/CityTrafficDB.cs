using CityTraffic.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityTraffic.DAL
{
    public class CityTrafficDB : DbContext
    {
        public CityTrafficDB(DbContextOptions<CityTrafficDB> options) : base(options) { }

        public DbSet<TransportRoute> TransportRoutes {get;set;}
        public DbSet<Stoppoint> Stoppoints { get;set;}
        public DbSet<FavoritesTransportRoute> FavoritesTransportRoutes { get;set;}
        public DbSet<FavoritesStoppoint> FavoritesStoppoints { get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransportRoute>(
                transportRoute => 
                {
                    transportRoute.HasKey(p => p.Id);

                    transportRoute.Property(p => p.Id)
                        .HasColumnName("id")
                        .HasColumnType("INTEGER")
                        .ValueGeneratedOnAdd()
                        .IsRequired(true);

                    transportRoute.Property(p => p.RouteId)
                        .HasColumnName("route_id")
                        .HasColumnType("TEXT")
                        .IsRequired(false);

                    transportRoute.Property(p => p.RouteNumber)
                        .HasColumnName("route_number")
                        .HasColumnType("TEXT")
                        .IsRequired(false);

                    transportRoute.Property(p => p.Title)
                        .HasColumnName("title")
                        .HasColumnType("TEXT")
                        .IsRequired(false);

                    transportRoute.Property(p => p.RouteTypeId)
                        .HasColumnName("route_type_id")
                        .HasColumnType("INTEGER")
                        .IsRequired(true);

                    transportRoute
                        .HasOne(e => e.FavoritesTransportRoute)
                        .WithOne(e => e.TransportRoute)
                        .HasForeignKey<FavoritesTransportRoute>(e => e.TransportRouteId)
                        .IsRequired(true);
                });

            modelBuilder.Entity<FavoritesTransportRoute>(
                favoriteTransportRoute =>
                {
                    favoriteTransportRoute.HasKey(p => p.Id);

                    favoriteTransportRoute.Property(p => p.Id)
                        .HasColumnName("id")
                        .HasColumnType("INTEGER")
                        .ValueGeneratedOnAdd()
                        .IsRequired(true);

                    favoriteTransportRoute.Property(p => p.TransportRouteId)
                        .HasColumnName("transport_route_id")
                        .HasColumnType("INTEGER")
                        .IsRequired(true);
                        
                });

            modelBuilder.Entity<Stoppoint>(
                stoppoint =>
                {
                    stoppoint.HasKey(p => p.Id);

                    stoppoint.Property(p => p.Id)
                        .HasColumnName("id")
                        .HasColumnType("INTEGER")
                        .ValueGeneratedOnAdd()
                        .IsRequired(true);

                    stoppoint.Property(p => p.StoppointId)
                        .HasColumnName("stoppoint_id")
                        .HasColumnType("INTEGER")
                        .IsRequired(true);

                    stoppoint.Property(p => p.StoppointName)
                        .HasColumnName("stoppoint_name")
                        .HasColumnType("TEXT")
                        .IsRequired(false);

                    stoppoint.Property(p => p.Location)
                        .HasColumnName("location")
                        .HasColumnType("TEXT")
                        .IsRequired(false);


                    stoppoint.Property(p => p.Note)
                        .HasColumnName("note")
                        .HasColumnType("TEXT")
                        .IsRequired(false);

                    stoppoint
                        .HasOne(e => e.FavoritesStoppoint)
                        .WithOne(e => e.Stoppoint)
                        .HasForeignKey<FavoritesStoppoint>(e => e.StoppointId)
                        .IsRequired(true);
                });

            modelBuilder.Entity<FavoritesStoppoint>(
                favoriteStoppoints =>
                {
                    favoriteStoppoints.HasKey(p => p.Id);

                    favoriteStoppoints.Property(p => p.Id)
                        .HasColumnName("id")
                        .HasColumnType("INTEGER")
                        .ValueGeneratedOnAdd()
                        .IsRequired(true);

                    favoriteStoppoints.Property(p => p.StoppointId)
                        .HasColumnName("stoppoint_id")
                        .HasColumnType("INTEGER")
                        .IsRequired(true);
                });
        }
    }
}
