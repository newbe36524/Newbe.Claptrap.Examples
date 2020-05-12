using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Bootstrapper;
using Newbe.Claptrap.Shop.Grains.Domain.Sku.Master;
using Newbe.Claptrap.Shop.Grains.Modules;
using Newbe.Claptrap.Shop.Repository.Modules;
using Orleans.Configuration;
using Orleans.Configuration.Overrides;

namespace Newbe.Claptrap.Shop
{
    class Program
    {
        public static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(context =>
                {
                    var serviceProviderFactory = new AutofacServiceProviderFactory(
                        builder =>
                        {
                            var collection = new ServiceCollection().AddLogging(logging =>
                            {
                                logging.AddConsole();
                                logging.SetMinimumLevel(LogLevel.Debug);
                                logging.AddFilter((s, level) => s.StartsWith("Orleans") && level >= LogLevel.Warning);
                                logging.AddFilter((s, level) => s.Contains("Claptrap"));
                            });
                            builder.RegisterModule<RepositoryModule>();
                            builder.RegisterModule<GrainsModule>();
                            var buildServiceProvider = collection.BuildServiceProvider();
                            var loggerFactory = buildServiceProvider.GetService<ILoggerFactory>();
                            var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory, builder);
                            var claptrapBootstrapper = bootstrapperBuilder
                                .ScanClaptrapModule()
                                .UseSQLiteAsEventStore()
                                .UseSQLiteAsStateStore()
                                .ScanClaptrapDesigns(new[]
                                {
                                    typeof(SkuGrain).Assembly
                                })
                                .Build();
                            claptrapBootstrapper.Boot();
                        });

                    return serviceProviderFactory;
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .ConfigureApplicationParts(manager =>
                            manager.AddFromDependencyContext().WithReferences())
                        ;
                })
                .RunConsoleAsync();
        }
    }
}