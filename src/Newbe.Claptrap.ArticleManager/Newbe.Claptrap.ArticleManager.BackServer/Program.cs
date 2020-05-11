using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.ArticleManager.Grains;
using Newbe.Claptrap.Bootstrapper;
using Orleans;
using Orleans.Hosting;

namespace Newbe.Claptrap.ArticleManager.BackServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new SiloHostBuilder();

            hostBuilder
                .UseLocalhostClustering()
                .UseServiceProviderFactory(collection =>
                {
                    collection.AddLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.SetMinimumLevel(LogLevel.Debug);
                        logging.AddFilter((s, level) => s.StartsWith("Orleans") && level >= LogLevel.Warning);
                        logging.AddFilter((s, level) => s.Contains("Claptrap"));
                    });
                    var builder = new ContainerBuilder();

                    builder.Populate(collection);

                    var buildServiceProvider = collection.BuildServiceProvider();
                    var loggerFactory = buildServiceProvider.GetService<ILoggerFactory>();
                    var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory, builder);
                    var claptrapBootstrapper = bootstrapperBuilder
                        .ScanClaptrapModule()
                        .UseSQLiteAsEventStore()
                        .UseSQLiteAsStateStore()
                        .ScanClaptrapDesigns(new[]
                        {
                            typeof(ArticleGrain).Assembly
                        })
                        .Build();
                    claptrapBootstrapper.Boot();


                    var container = builder.Build();
                    var serviceProvider = new AutofacServiceProvider(container);
                    return serviceProvider;
                })
                .ConfigureApplicationParts(manager =>
                    manager.AddFromDependencyContext().WithReferences())
                ;
            var siloHost = hostBuilder.Build();
            Console.WriteLine("server starting");
            await siloHost.StartAsync();
            Console.WriteLine("server started");

            Console.ReadLine();
        }
    }
}