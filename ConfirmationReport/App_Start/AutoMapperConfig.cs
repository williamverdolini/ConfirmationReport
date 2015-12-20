using System.Linq;
using AutoMapper;
using Castle.Windsor;
using ConfirmRep.Profiles;

namespace ConfirmRep.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure(IWindsorContainer container)
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration, container));
        }

        private static void GetConfiguration(IConfiguration configuration, IWindsorContainer container)
        {
            var profiles = typeof(UserMapperProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            foreach (var profile in profiles)
            {
                configuration.AddProfile(container.Resolve(profile) as Profile);
            }
        }
    }
}