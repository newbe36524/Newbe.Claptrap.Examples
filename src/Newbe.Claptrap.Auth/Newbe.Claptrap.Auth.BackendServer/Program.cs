using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Formatters.Prometheus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.AppMetrics;
using Newbe.Claptrap.Auth.Grains;
using Newbe.Claptrap.Auth.Repository;
using Newbe.Claptrap.Bootstrapper;
using Newbe.Claptrap.DesignStoreFormatter;
using Newbe.Claptrap.StorageProvider.Relational;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Newbe.Claptrap.Auth.BackendServer
{
    public class Program
    {
        private static Timer _timer;

        public static void Main(string[] args)
        {
            ClaptrapMetrics.MetricsRoot = new MetricsBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Report
                .ToInfluxDb(options =>
                {
                    options.InfluxDb.Database = "metricsdatabase";
                    options.InfluxDb.CreateDataBaseIfNotExists = true;
                    options.InfluxDb.UserName = "claptrap";
                    options.InfluxDb.Password = "claptrap";
                    options.InfluxDb.BaseUri = new Uri("http://127.0.0.1:19086");
                    options.InfluxDb.CreateDataBaseIfNotExists = true;
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                    options.FlushInterval = TimeSpan.FromSeconds(20);
                })
                .Build();

            _timer = new Timer(1000);
            _timer.Elapsed += (sender, eventArgs) =>
            {
                Task.WhenAll(ClaptrapMetrics.MetricsRoot.ReportRunner.RunAllAsync()).Wait();
            };
            _timer.Start();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(context =>
                {
                    var serviceProviderFactory = new AutofacServiceProviderFactory(
                        builder =>
                        {
                            builder.RegisterModule<RepositoryModule>();
                            
                            var collection = new ServiceCollection().AddLogging(logging =>
                            {
                                var configurationSection = context.Configuration.GetSection("Logging");
                                logging.AddConfiguration(configurationSection);
                                logging.AddConsole();
                            });
                            var buildServiceProvider = collection.BuildServiceProvider();
                            var loggerFactory = buildServiceProvider.GetService<ILoggerFactory>();
                            var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory, builder);
                            var config = context.Configuration.GetSection("Claptrap");
                            var claptrapConfig = new Models.ClaptrapClusteringOptions();
                            config.Bind(claptrapConfig);
                            var claptrapBootstrapper = bootstrapperBuilder
                                .ScanClaptrapModule()
                                .ScanClaptrapDesigns(new[]
                                {
                                    typeof(UserGrain).Assembly
                                })
                                .AddConnectionString(Defaults.ConnectionName,
                                    claptrapConfig.DefaultConnectionString)
                                .UseMySql(mysql =>
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
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .ConfigureDefaults()
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
                                options.GatewayPort = defaultGatewayPort;
                                options.SiloPort = defaultSiloPort;
                                options.AdvertisedIPAddress = ip;
                                options.GatewayListeningEndpoint = new IPEndPoint(ip,
                                    claptrapOptionsOrleans.GatewayPort ?? defaultGatewayPort);
                                options.SiloListeningEndpoint = new IPEndPoint(ip,
                                    claptrapOptionsOrleans.SiloPort ?? defaultSiloPort);
                            });
                        })
                        .UseAdoNetClustering(options =>
                        {
                            var claptrapOptions = BindClaptrapOptions();
                            var clustering = claptrapOptions.Orleans.Clustering;
                            options.Invariant = clustering.Invariant;
                            options.ConnectionString = clustering.ConnectionString;
                        })
                        .ConfigureApplicationParts(manager =>
                            manager.AddFromDependencyContext().WithReferences())
                        .UseDashboard(options => options.Port = 9000);

                    Models.ClaptrapClusteringOptions BindClaptrapOptions()
                    {
                        var webHostBuilderContext =
                            (WebHostBuilderContext) siloBuilder.Properties[
                                typeof(WebHostBuilderContext)];
                        var config = webHostBuilderContext.Configuration.GetSection("Claptrap");
                        var claptrapOptions = new Models.ClaptrapClusteringOptions();
                        config.Bind(claptrapOptions);
                        return claptrapOptions;
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureMetrics(ClaptrapMetrics.MetricsRoot)
                .UseMetrics(
                    options =>
                    {
                        options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter = ClaptrapMetrics.MetricsRoot
                                .OutputMetricsFormatters
                                .OfType<MetricsPrometheusTextOutputFormatter>()
                                .First();
                            endpointsOptions.MetricsEndpointOutputFormatter = ClaptrapMetrics.MetricsRoot
                                .OutputMetricsFormatters
                                .OfType<MetricsPrometheusProtobufOutputFormatter>()
                                .First();
                        };
                    });
    }
}