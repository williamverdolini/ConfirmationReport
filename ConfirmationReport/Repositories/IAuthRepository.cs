using System.Threading.Tasks;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using Microsoft.AspNet.Identity;

namespace ConfirmRep.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(RegisterUserViewModel userModel);        
        Task<ApplicationUser> FindUser(string userName, string password);
        Task<ApplicationUser> GetUserInfo(string id);
        Task<ApplicationUser> GetUserInfoByUsername(string userName);
    }
}
