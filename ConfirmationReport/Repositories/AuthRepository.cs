using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ConfirmRep.Repositories
{
    public class CustomAuthRepository : IAuthRepository
    {
        public async Task<IdentityResult> RegisterUser(RegisterUserViewModel userModel)
        {
            using(AuthContext _ctx = new AuthContext())
            using(UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx))){
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userModel.UserName,
                    Name = userModel.Name,
                    Surname = userModel.Surname
                };

                user.Claims.Add(new IdentityUserClaim() { ClaimType = ClaimTypes.Role, ClaimValue = "user", UserId = user.Id });
                if (userModel.Roles != null)
                {
                    userModel.Roles.ToList().ForEach(r =>
                    {
                        user.Claims.Add(new IdentityUserClaim() { ClaimType = ClaimTypes.Role, ClaimValue = r, UserId = user.Id });
                    });
                }
                try
                {
                    var result = await _userManager.CreateAsync(user, userModel.Password);
                    return result;
                }
                catch (DbEntityValidationException ex)
                {
                    ex.EntityValidationErrors.ToList().ForEach(er =>
                    {
                        er.ValidationErrors.ToList().ForEach(ver =>
                        {
                            System.Diagnostics.Debug.WriteLine(ver.PropertyName + ": " + ver.ErrorMessage);
                        });
                    });

                    throw ex;
                }
            }

        }
        
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            using(AuthContext _ctx = new AuthContext())
            using (UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx)))
            {
                ApplicationUser user = await _userManager.FindAsync(userName, password);
                return user;
            }
        }

        public async Task<ApplicationUser> GetUserInfo(string id)
        {
            using (AuthContext _ctx = new AuthContext())
            using (UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx)))
            {
                return await _userManager.FindByIdAsync(id);
            }
        }

        public async Task<ApplicationUser> GetUserInfoByUsername(string userName)
        {
            using (AuthContext _ctx = new AuthContext())
            using (UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx)))
            {
                return await _userManager.FindByNameAsync(userName);
            }
        }
    }
}