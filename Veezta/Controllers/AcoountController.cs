using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Veezta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcoountController : Controller
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;

        public AcoountController(SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("GoogleLogin")]
        public IActionResult GoogleLogin()
        {
            // Redirect to Google for authentication
            string redirectUrl = Url.Action(nameof(ExternalLoginInfo), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }


        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("Error loading external login information.");

            // Sign in the user with this external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // User successfully authenticated
                return Ok();
            }

            // If the user does not have an account, then create one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new CustomUser { UserName = email, Email = email };
            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddLoginAsync(user, info);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }
            }

            return BadRequest("Could not create an account.");
        }


    }
}
