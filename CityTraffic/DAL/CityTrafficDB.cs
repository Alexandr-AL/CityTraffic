using CityTraffic.Models.Entities;
using CityTraffic.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public async void InitDB()
        {
            await Database.MigrateAsync();

            if (!TransportRoutes.Any())
            {
                var allTransportRoutes = await GortransPermAPI.GetAllTransportRoutes();
                foreach (var item in allTransportRoutes)
                {
                    await TransportRoutes.AddAsync(new TransportRoute
                    {
                        RouteId = item.RouteId,
                        RouteNumber = item.RouteNumber,
                        Title = item.Title,
                        RouteTypeId = item.RouteTypeId
                    });
                }
            }

            if (!Stoppoints.Any() && TransportRoutes.Any())
            {
                var allStoppoints = await GortransPermAPI.GetAllStoppoints(TransportRoutes.AsEnumerable());
                foreach (var item in allStoppoints)
                {
                    await Stoppoints.AddAsync(new Stoppoint
                    {
                        StoppointId = item.StoppointId,
                        StoppointName = item.StoppointName,
                        Location = item.Location,
                        Note = item.Note
                    });
                }
            }

            await SaveChangesAsync();

            Debug.WriteLine("--------------------\nInit DB is done\n--------------------");
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
                else TransportRoutes.Add(new TransportRoute
                {
                    RouteId = newItem.RouteId,
                    RouteNumber = newItem.RouteNumber,
                    Title = newItem.Title,
                    RouteTypeId = newItem.RouteTypeId
                });
            }

            foreach (var itemToRemove in routesId)
                TransportRoutes.Remove(TransportRoutes.Include(tr => tr.FavoritesTransportRoute).Single(e => e.RouteId == itemToRemove));
            
            /*Update Stoppoints*/
            var stoppointsNew = await GortransPermAPI.GetAllStoppoints(transportRoutesNew);

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
                else Stoppoints.Add(new Stoppoint
                {
                    StoppointId = newItem.StoppointId,
                    StoppointName = newItem.StoppointName,
                    Location = newItem.Location,
                    Note = newItem.Note
                });
            }

            foreach (var itemToRemove in stoppointId)
                Stoppoints.Remove(Stoppoints.Include(sp => sp.FavoritesStoppoint).Single(e => e.StoppointId == itemToRemove));

            timer.Stop();

            Debug.WriteLine($"--------------------\nUpdate DB completed in {timer.Elapsed.TotalSeconds} sec\n--------------------");

            return (await SaveChangesAsync(), timer.Elapsed.TotalSeconds);
        }
    }
}
