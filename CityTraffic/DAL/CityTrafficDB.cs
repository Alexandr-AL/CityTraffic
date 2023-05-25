using CityTraffic.Models.Entities;
using CityTraffic.Models.Interfaces;
using CityTraffic.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

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

        public async Task InitDB()
        {
            await Database.MigrateAsync();

            if (!TransportRoutes.Any())
                await TransportRoutes.AddRangeAsync(await GortransPermAPI.GetAllTransportRoutes());

            if (!Stoppoints.Any() && TransportRoutes.Any())
                await Stoppoints.AddRangeAsync(await GortransPermAPI.GetAllStoppoints(TransportRoutes.AsEnumerable()));

            await SaveChangesAsync();
        }

        public async Task<(int, double)> UpdateDB()
        {
            var timer = new Stopwatch();
            timer.Start();

            /*Update TransportRoutes*/
            var transportRoutesNew = await GortransPermAPI.GetAllTransportRoutes();

            List<string> routesId = new(TransportRoutes.Select(tr => tr.RouteId).ToList());

            foreach (var newItem in transportRoutesNew)
            {
                if (routesId.Contains(newItem.RouteId))
                {
                    routesId.Remove(newItem.RouteId);

                    var transportRoute = TransportRoutes.Single(tr => tr.RouteId == newItem.RouteId);

                    if (newItem.Equals(transportRoute)) continue;

                    transportRoute.RouteNumber = newItem.RouteNumber;
                    transportRoute.Title = newItem.Title;
                    transportRoute.RouteTypeId = newItem.RouteTypeId;
                }
                else TransportRoutes.Add(newItem);
            }

            foreach (var itemToRemove in routesId)
                TransportRoutes.Remove(TransportRoutes.Include(tr => tr.FavoritesTransportRoute).Single(e => e.RouteId == itemToRemove));
            
            /*Update Stoppoints*/
            var stoppointsNew = new List<Stoppoint>();
            if (transportRoutesNew.Any())
                stoppointsNew = await GortransPermAPI.GetAllStoppoints(transportRoutesNew);

            List<int> stoppointId = new(Stoppoints.Select(sp => sp.StoppointId).ToList());

            foreach (var newItem in stoppointsNew)
            {
                if (stoppointId.Contains(newItem.StoppointId))
                {
                    stoppointId.Remove(newItem.StoppointId);

                    var stoppoint = Stoppoints.Single(sp => sp.StoppointId == newItem.StoppointId);

                    if (newItem.Equals(stoppoint)) continue;

                    stoppoint.StoppointName = newItem.StoppointName;
                    stoppoint.Location = newItem.Location;
                    stoppoint.Note = newItem.Note;
                }
                else Stoppoints.Add(newItem);
            }

            foreach (var itemToRemove in stoppointId)
                Stoppoints.Remove(Stoppoints.Include(sp => sp.FavoritesStoppoint).Single(e => e.StoppointId == itemToRemove));

            timer.Stop();

            return (await SaveChangesAsync(), timer.Elapsed.TotalSeconds);
        }
    }
}
