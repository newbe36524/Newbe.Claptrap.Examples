using System;
using Autofac;

namespace Newbe.Claptrap.Shop.Repository.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DbConnectionStringFactory>()
                .As<IDbConnectionStringFactory>()
                .SingleInstance();
            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .SingleInstance();
            builder.RegisterType<DbManager>()
                .As<IDbManager>()
                .SingleInstance();
            builder.RegisterType<SkuRepository>()
                .As<ISkuRepository>()
                .InstancePerLifetimeScope();
        }
    }
}