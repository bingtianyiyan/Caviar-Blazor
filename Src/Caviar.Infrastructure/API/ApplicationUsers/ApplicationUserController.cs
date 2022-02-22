using Caviar.Infrastructure.API.BaseApi;
using Caviar.SharedKernel.Entities.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Caviar.SharedKernel.Entities.User;
using Caviar.SharedKernel.Entities;

namespace Caviar.Infrastructure.API
{
    public partial class ApplicationUserController : EasyBaseApiController<ApplicationUserView,ApplicationUser>
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        public virtual async Task<IActionResult> Login(UserLogin login, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null) return BadRequest("Username and password are invalid");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Username and password are invalid");
            await _signInManager.SignInAsync(user, login.RememberMe);
            return Ok(title: "Login Succeeded",url:returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> SignInActual(string t)
        {
            var data = t;
            var parts = data.Split('|');

            var identityUser = await _userManager.FindByIdAsync(parts[0]);

            var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "SignIn", parts[1]);

            if (isTokenValid)
            {
                await _signInManager.SignInAsync(identityUser, true);
                if (parts.Length == 3 && Url.IsLocalUrl(parts[2]))
                {
                    return Redirect(parts[2]);
                }
                return Redirect("/");
            }
            else
            {
                return Unauthorized("Validation failed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(url:"/");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutServer()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult CurrentUserInfo()
        {
            var currentUser = new CurrentUser
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims.Select(u=>(new CaviarClaim(u)))
            };
            return Ok(currentUser);
        }

    }
}