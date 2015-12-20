using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models.View;
using ConfirmRep.Repositories;
using Microsoft.AspNet.Identity;

namespace ConfirmRep.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IAuthRepository repo;
        private readonly IMappingEngine mapper;

        // TODO: Move into a worker service
        public AccountController(IAuthRepository repo, IMappingEngine mapper)
        {
            Contract.Requires<ArgumentNullException>(repo != null, "IAuthRepository");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            this.repo = repo;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await repo.RegisterUser(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [Authorize]
        [ResponseType(typeof(UserViewModel))]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var user = await repo.GetUserInfo(User.Identity.GetUserId());
            if (user == null)
                return NotFound();
            return Ok(mapper.Map<UserViewModel>(user));
        }        

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
