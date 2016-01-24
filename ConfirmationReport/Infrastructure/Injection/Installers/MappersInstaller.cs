using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ConfirmRep.Infrastructure.Common;

namespace ConfirmRep.Infrastructure.Injection.Installers
{
    public class MappersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMappingEngine>().UsingFactoryMethod(() => Mapper.Engine));
            container.Register(Component.For<IMapper>().ImplementedBy<LightMapper>());
            container.Register(Classes.FromThisAssembly().BasedOn<Profile>().Configure(c => c.LifestyleTransient()));
        }
    }
}