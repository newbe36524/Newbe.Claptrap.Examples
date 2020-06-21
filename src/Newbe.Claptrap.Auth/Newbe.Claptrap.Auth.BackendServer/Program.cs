using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Auth.Grains;
using Newbe.Claptrap.Auth.Models;
using Newbe.Claptrap.Auth.Repository;
using Newbe.Claptrap.Bootstrapper;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.StorageProvider.Relational;
using NLog.Web;
using Orleans;
using Orleans.Hosting;

namespace Newbe.Claptrap.Auth.BackendServer
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
                .UseOrleansClaptrap()
                .UseOrleans((context, builder) =>
                {
                    var config = context.Configuration.GetSection(ClaptrapServeringOptions.ConfigurationSectionName);
                    var claptrapConfig = new ClaptrapClusteringOptions();
                    config.Bind(claptrapConfig);
                    if (claptrapConfig?.Orleans?.EnableDashboard == true)
                    {
                        builder.UseDashboard(options => options.Port = 9000);
                    }
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseServiceProviderFactory(context =>
                {
                    var serviceProviderFactory = new AutofacServiceProviderFactory(
                        builder =>
                        {
                            builder.RegisterModule<RepositoryModule>();

                            var collection = new ServiceCollection().AddLogging(logging =>
                            {
                                logging.SetMinimumLevel(LogLevel.Debug);
                            });
                            var buildServiceProvider = collection.BuildServiceProvider();
                            var loggerFactory = buildServiceProvider.GetService<ILoggerFactory>();
                            var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory, builder);

                            var claptrapBootstrapper = bootstrapperBuilder
                                .ScanClaptrapModule()
                                .AddDefaultConfiguration(context)
                                .ScanClaptrapDesigns(new[]
                                {
                                    typeof(UserGrain).Assembly
                                })
                                .UsePostgreSQL(mysql =>
                                    mysql
                                        .AsEventStore(eventStore =>
                                            eventStore.SharedTable())
                                        .AsStateStore(stateStore =>
                                            stateStore.SharedTable())
                                )
                                .Build();
                            claptrapBootstrapper.Boot();
                        });


                    return serviceProviderFactory;
                })
#if !DEBUG
                .UseOrleans((context, siloBuilder) =>
                {
                    siloBuilder
                        .UseConsulClustering(options =>
                        {
                            options.Address =
                                new Uri(context.Configuration["Claptrap:Orleans:Clustering:ConsulUrl"]);
                        })
                        .ConfigureApplicationParts(manager =>
                            manager.AddFromDependencyContext().WithReferences());
                })
#endif
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}