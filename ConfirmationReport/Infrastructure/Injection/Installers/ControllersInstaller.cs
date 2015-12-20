using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.ServiceWorkers;

namespace ConfirmRep.Infrastructure.Injection.Installers
{
    /// <summary>
    /// Windsor.Castle ControllerInstaller
    /// see http://docs.castleproject.org/Windsor.Windsor-tutorial-ASP-NET-MVC-3-application-To-be-Seen.ashx
    /// </summary>
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            // Register WebAPI Controllers
            container.Register(
                Classes
                .FromThisAssembly()
                .BasedOn<IHttpController>()
                .LifestyleTransient());

            container.Register(Component.For<IConfirmationReportWorker>().ImplementedBy<ConfirmationReportWorker>().LifeStyle.Transient);
        }

    }
}