using Autofac;
using Newbe.Claptrap.Ticketing.LoadTestClient.Services;

namespace Newbe.Claptrap.Ticketing.LoadTestClient
{
    public class LoadTestClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LoadTestService>()
                .As<ILoadTestService>()
                .SingleInstance();
        }
    }
}