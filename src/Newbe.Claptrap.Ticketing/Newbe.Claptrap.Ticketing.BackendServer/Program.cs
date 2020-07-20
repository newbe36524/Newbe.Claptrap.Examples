using System;
using Newbe.Claptrap.Ticketing.IActor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Bootstrapper;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main;
using Newbe.Claptrap.Ticketing.Actors.Seat.Minions.SeatUpdateCount;
using Newbe.Claptrap.Ticketing.Actors.Train.Main;
using Newbe.Claptrap.Ticketing.Models;
using NLog.Web;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Newbe.Claptrap.Ticketing.BackendServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseClaptrap(builder =>
                {
                    builder.ScanClaptrapDesigns(new[]
                        {
                            typeof(ISeatGrain).Assembly,
                            typeof(SeatGrain).Assembly
                        })
                        .ConfigureClaptrapDesign(x =>
                            x.ClaptrapOptions.EventCenterOptions.EventCenterType = EventCenterType.OrleansClient)
                        .ConfigureClaptrapDesign(
                            x => x.ClaptrapTypeCode == ClaptrapCodes.SeatUpdateCountMinionGrain,
                            x => x.ClaptrapOptions.StateSavingOptions.SavingWindowVersionLimit = 1);
                })
                .UseOrleansClaptrap()
                .UseOrleans(builder =>
                {
                    builder
                        .Configure<GrainCollectionOptions>(options =>
                        {
                            options.CollectionAge = TimeSpan.FromMinutes(5);
                            options.ClassSpecificCollectionAge[typeof(TrainGran).FullName!]
                                = TimeSpan.FromHours(2);
                            options.ClassSpecificCollectionAge[typeof(SeatGrain).FullName!]
                                = TimeSpan.FromSeconds(1);
                            options.ClassSpecificCollectionAge[typeof(SeatUpdateCountMinionGrain).FullName!]
                                = TimeSpan.FromSeconds(1);
                        })
                        .UseDashboard(options => options.Port = 9000);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}