using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;

namespace ConfirmRep.Profiles
{
    public class UserMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.ResolveUsing<RoleResolver>());
        }
    }

    public class RoleResolver : ValueResolver<ApplicationUser, IList<string>>
    {
        protected override IList<string> ResolveCore(ApplicationUser user)
        {
            List<string> roles = new List<string>();
            user.Claims.Where(c => c.ClaimType.Equals(ClaimTypes.Role)).ToList().ForEach(c => roles.Add(c.ClaimValue));
            return roles;
        }
    }
}