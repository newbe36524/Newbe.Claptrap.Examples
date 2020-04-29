using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.ArticleManager.Grains;
using Newbe.Claptrap.Preview.Impl.Bootstrapper;
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
                    var claptrapBootstrapperFactory = new AutofacClaptrapBootstrapperFactory(loggerFactory);
                    var claptrapBootstrapper = claptrapBootstrapperFactory.Create(new[]
                    {
                        typeof(ArticleGrain).Assembly
                    });
                    claptrapBootstrapper.RegisterServices(builder);

                   
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