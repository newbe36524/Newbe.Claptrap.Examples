using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Bootstrapper;

namespace Newbe.Claptrap.Standalone
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = CreateLoggerFactory();
            var containerBuilder = new ContainerBuilder();
            var claptrapBootstrapper = CreateClaptrapBootstrapper(loggerFactory, containerBuilder);
            claptrapBootstrapper.Boot();
            var container = BootClaptrapAndCreateContainer(containerBuilder);
            await using var scope = container.BeginLifetimeScope();
            await RunTest(scope, loggerFactory);
        }

        private static async Task RunTest(IComponentContext scope, ILoggerFactory loggerFactory)
        {
            var factory = scope.Resolve<AccountClaptrap.Factory>();

            var accountClaptrap = factory.Invoke(new ClaptrapIdentity("123", ClaptrapCodes.Account));

            var logger = loggerFactory.CreateLogger<Program>();
            await accountClaptrap.ActivateAsync();
            var balance = await accountClaptrap.GetBalanceAsync();
            logger.LogInformation("balance now is {balance}",
                balance);
            const int diff = +100;
            await accountClaptrap.ChangeBalanceAsync(diff);
            balance = await accountClaptrap.GetBalanceAsync();
            logger.LogInformation("balance now is {balance} after make a change as {diff} diff.",
                balance,
                diff);

            const int times = 500;
            logger.LogInformation("start to add balance concurrently by {times} times, add {diff} each time",
                times,
                diff);

            var sw = Stopwatch.StartNew();
            await Task.WhenAll(Enumerable.Repeat(0, times).Select(i => accountClaptrap.ChangeBalanceAsync(diff)));

            logger.LogInformation(
                "expect final balance will be {expected} , and actually final balance is {balance}. cost {time} ms",
                balance + times * diff,
                await accountClaptrap.GetBalanceAsync(),
                sw.ElapsedMilliseconds);
        }

        private static IContainer BootClaptrapAndCreateContainer(
            ContainerBuilder builder)
        {
            builder.Populate(new ServiceCollection().AddLogging(logging => { logging.AddConsole(); }));
            builder.RegisterType<AccountClaptrap>()
                .AsSelf()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            return container;
        }

        private static IClaptrapBootstrapper CreateClaptrapBootstrapper(ILoggerFactory loggerFactory,
            ContainerBuilder builder)
        {
            var claptrapBootstrapperBuilder =
                new AutofacClaptrapBootstrapperBuilder(loggerFactory, builder);
            var bootstrapper = claptrapBootstrapperBuilder
                .ScanClaptrapModule()
                .ScanClaptrapDesigns(new[]
                {
                    typeof(IAccountClaptrap).Assembly
                })
                .UseSQLiteAsTestingStorage()
                .Build();
            return bootstrapper;
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            var collection = new ServiceCollection();
            collection.AddLogging(logging => { logging.AddConsole(); });
            var buildServiceProvider = collection.BuildServiceProvider();
            var loggerFactory = buildServiceProvider.GetService<ILoggerFactory>();
            return loggerFactory;
        }
    }
}