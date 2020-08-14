using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Ticketing.LoadTestClient.Services;
using Newbe.Claptrap.Ticketing.Repository.Module;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Newbe.Claptrap.Ticketing.LoadTestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var services = new ServiceCollection();
                services.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddNLog();
                });
                logger.Debug("init main");
                var builder = new ContainerBuilder();
                builder.Populate(services);
                builder.RegisterModule<LoadTestClientModule>();
                builder.RegisterModule<RepositoryModule>();
                var container = builder.Build();
                var loadTestService = container.Resolve<ILoadTestService>();
                await loadTestService.RunAsync();
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
                LogManager.Shutdown();
            }
        }
    }
}