using Autofac;
using HelloClaptrap.Repository.Impl;

namespace HelloClaptrap.Repository
{
    public class DbFactoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .SingleInstance();
        }
    }
}