using Autofac;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Shop.Grains.Modules
{
    public class GrainsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(typeof(GrainsModule).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract)
                .Where(x => x.GetInterface(typeof(IClaptrapGrain).FullName) != null)
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}