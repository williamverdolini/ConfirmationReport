using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Repositories;

namespace ConfirmRep.Infrastructure.Injection.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            container.Register(Component.For<ConfirmReportContext>().LifeStyle.Transient);
            container.Register(Component.For<IAuthRepository>().ImplementedBy<CustomAuthRepository>().LifeStyle.Transient);
            container.Register(Component.For<IConfirmationReportRepository>().ImplementedBy<ConfirmationReportRepository>().LifeStyle.Transient);
            
        }
    }
}