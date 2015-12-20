using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;
using ConfirmRep.Repositories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace ConfirmRep.Providers
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthRepository _repo;

        public CustomAuthorizationServerProvider(IAuthRepository _repo)
        {
            this._repo = _repo;
        }

        // Responsible for validating the “Client”
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult(0).ConfigureAwait(false);
        }

        // Responsible to validate the username and password sent to the authorization server’s token endpoint
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            ApplicationUser user = await _repo.FindUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

            context.Validated(identity);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}