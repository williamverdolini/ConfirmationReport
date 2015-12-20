using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.Windsor;
using ConfirmRep.App_Start;
using ConfirmRep.Infrastructure.Injection.Installers;
using ConfirmRep.Infrastructure.Injection.WebAPI;
using ConfirmRep.Providers;
using ConfirmRep.Repositories;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

//see: http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
[assembly: OwinStartup(typeof(ConfirmRep.Startup))]

namespace ConfirmRep
{
    public class Startup
    {
        private readonly IWindsorContainer container;

        public Startup()
        {
            this.container = new WindsorContainer()
                            .Install(
                                    new ControllersInstaller(),
                                    new RepositoriesInstaller(),
                                    new MappersInstaller()
                                    );
        }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);
            
            // Configure WebApi
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            config.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(this.container));

            // Configure all AutoMapper Profiles
            AutoMapperConfig.Configure(container);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                //Provider = new SimpleAuthorizationServerProvider()
                Provider = new CustomAuthorizationServerProvider(container.Resolve<IAuthRepository>())
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
