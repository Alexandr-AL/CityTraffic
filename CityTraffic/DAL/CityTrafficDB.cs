using CityTraffic.Models.Entities;
using CityTraffic.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CityTraffic.DAL
{
    public class CityTrafficDB : DbContext
    {
        public CityTrafficDB(DbContextOptions<CityTrafficDB> options) : base(options) { }

        public DbSet<EntityTransportRoute> TransportRoutes {get;set;}
        public DbSet<EntityStoppoint> Stoppoints { get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityTransportRoute>(entityTR =>
            {
                entityTR.HasKey(x => x.RouteId);

                entityTR.Property(x => x.RouteId).HasColumnName("route_id").IsRequired();
                entityTR.Property(x => x.RouteNumber).HasColumnName("route_number");
                entityTR.Property(x => x.RouteTypeId).HasColumnName("route_type_id");
                entityTR.Property(x => x.Title).HasColumnName("title");
                entityTR.Property(x => x.FavoriteTransportRoute).HasColumnName("favorite_transport_route");
            });

            modelBuilder.Entity<EntityStoppoint>(entitySt =>
            {
                entitySt.HasKey(x => x.StoppointId);

                entitySt.Property(x => x.StoppointId).HasColumnName("stoppoint_id").IsRequired();
                entitySt.Property(x => x.StoppointName).HasColumnName("stoppoint_name");
                entitySt.Property(x => x.Location).HasColumnName("location");
                entitySt.Property(x => x.Note).HasColumnName("note");
                entitySt.Property(x => x.FavoriteStoppoint).HasColumnName("favorite_stoppoint");
            });

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> InitDB()
        {
            await Database.MigrateAsync();

            if (!TransportRoutes.Any())
            {
                var allTransportRoutes = await GortransPermAPI.GetAllTransportRoutes();
                foreach (var item in allTransportRoutes)
                {
                    TransportRoutes.Add(new EntityTransportRoute
                    {
                        RouteId = item.RouteId,
                        RouteNumber = item.RouteNumber,
                        Title = item.Title,
                        RouteTypeId = item.RouteTypeId,
                        FavoriteTransportRoute = false
                    });
                }

                if (!Stoppoints.Any())
                {
                    var allStoppoints = await GortransPermAPI.GetAllStoppoints(allTransportRoutes);
                    foreach (var item in allStoppoints)
                    {
                        Stoppoints.Add(new EntityStoppoint
                        {
                            StoppointId = item.StoppointId,
                            StoppointName = item.StoppointName,
                            Location = item.Location,
                            Note = item.Note,
                            FavoriteStoppoint = false
                        });
                    }
                }
            }

            Debug.WriteLine("--------------------\nInit DB is done\n--------------------");

            return await SaveChangesAsync();
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
                else TransportRoutes.Add(new EntityTransportRoute
                {
                    RouteId = newItem.RouteId,
                    RouteNumber = newItem.RouteNumber,
                    Title = newItem.Title,
                    RouteTypeId = newItem.RouteTypeId
                });
            }

            if (routesId.Count > 0)
                foreach (var routeId in routesId)
                    TransportRoutes.Remove(TransportRoutes.Single(tr => tr.RouteId == routeId));
            
            /*Update Stoppoints*/
            var stoppointsNew = await GortransPermAPI.GetAllStoppoints(transportRoutesNew);

            List<int> stoppointsId = new(Stoppoints.Select(sp => sp.StoppointId).ToList());

            foreach (var newItem in stoppointsNew)
            {
                if (stoppointsId.Contains(newItem.StoppointId))
                {
                    stoppointsId.Remove(newItem.StoppointId);

                    var stoppoint = Stoppoints.Single(sp => sp.StoppointId == newItem.StoppointId);

                    if (newItem.Equals(stoppoint)) continue;

                    stoppoint.StoppointName = newItem.StoppointName;
                    stoppoint.Location = newItem.Location;
                    stoppoint.Note = newItem.Note;
                }
                else Stoppoints.Add(new EntityStoppoint
                {
                    StoppointId = newItem.StoppointId,
                    StoppointName = newItem.StoppointName,
                    Location = newItem.Location,
                    Note = newItem.Note
                });
            }

            if (stoppointsId.Count > 0)
                foreach (var stoppointId in stoppointsId)
                    Stoppoints.Remove(Stoppoints.Single(sp => sp.StoppointId == stoppointId));

            timer.Stop();

            Debug.WriteLine($"--------------------\nUpdate DB completed in {timer.Elapsed.TotalSeconds} sec\n--------------------");

            return (await SaveChangesAsync(), timer.Elapsed.TotalSeconds);
        }
    }
}
