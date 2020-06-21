using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Newbe.Claptrap.Auth.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(context =>
                {
                    var serviceProviderFactory = new AutofacServiceProviderFactory(
                        builder =>
                        {
                            var clientBuilder = new ClientBuilder();
                            clientBuilder
                                .UseLocalhostClustering()
#if !DEBUG
                                .UseConsulClustering(options =>
                                {
                                    options.Address =
                                        new Uri(context.Configuration["Claptrap:Orleans:Clustering:ConsulUrl"]);
                                })
#endif
                                .ConfigureApplicationParts(manager =>
                                    manager.AddFromDependencyContext().WithReferences())
                                ;
                            var clusterClient = clientBuilder.Build();
                            builder.RegisterInstance(clusterClient)
                                .As<IGrainFactory>()
                                .As<IClusterClient>()
                                .SingleInstance()
                                .ExternallyOwned();
                        });

                    return serviceProviderFactory;
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}