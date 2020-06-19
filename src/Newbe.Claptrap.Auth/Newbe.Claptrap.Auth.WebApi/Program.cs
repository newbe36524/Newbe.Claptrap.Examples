using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Auth.Models;
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
                                .ConfigureServices((context, services) =>
                                {
                                    services.Configure<EndpointOptions>(options =>
                                    {
                                        var claptrapOptions = BindClaptrapOptions();
                                        var claptrapOptionsOrleans = claptrapOptions.Orleans;
                                        var hostname = claptrapOptionsOrleans.Hostname ?? "localhost";
                                        var ip = hostname == "localhost"
                                            ? IPAddress.Loopback
                                            : Dns.GetHostEntry(hostname).AddressList.First();
                                        const int defaultGatewayPort = 30000;
                                        const int defaultSiloPort = 11111;
                                        var gatewayPort = claptrapOptionsOrleans.GatewayPort
                                                          ?? defaultGatewayPort;
                                        var siloPort = claptrapOptionsOrleans.SiloPort
                                                       ?? defaultSiloPort;
                                        options.GatewayPort = gatewayPort;
                                        options.SiloPort = siloPort;
                                        options.AdvertisedIPAddress = ip;
                                    });
                                })
                                .UseConsulClustering(options =>
                                {
                                    var claptrapOptions = BindClaptrapOptions();
                                    var clustering = claptrapOptions.Orleans.Clustering;
                                    options.Address = new Uri(clustering.ConsulUrl);
                                })
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

                    ClaptrapClusteringOptions BindClaptrapOptions()
                    {
                        var config = context.Configuration.GetSection("Claptrap");
                        var claptrapOptions = new ClaptrapClusteringOptions();
                        config.Bind(claptrapOptions);
                        return claptrapOptions;
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}