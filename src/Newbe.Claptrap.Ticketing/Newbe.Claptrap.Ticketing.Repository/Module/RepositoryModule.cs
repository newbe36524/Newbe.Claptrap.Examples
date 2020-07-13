﻿using Autofac;

namespace Newbe.Claptrap.Ticketing.Repository.Module
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LocationRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<TrainInfoRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}