using Autofac;
using HelloClaptrap.Repository.Impl;

namespace HelloClaptrap.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<DbFactoryModule>();
            builder.RegisterType<SkuRepository>()
                .As<ISkuRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();
        }
    }
}