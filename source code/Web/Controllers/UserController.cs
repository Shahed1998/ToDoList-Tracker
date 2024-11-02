using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.Helpers;
using Web.Models.Business_Entities;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl=null)
        {
            var model = new LoginViewModel();

            if (User.Identity?.IsAuthenticated == true)
            {
                if(!ReturnUrl.IsNullOrEmpty() && !Url.IsLocalUrl(ReturnUrl))
                {
                    Redirect(ReturnUrl!);
                }
                else
                {
                    RedirectToAction("Index", "Tracking");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!,
                    model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Tracking");
                }

                ModelState.AddModelError("LoginError", "Invalid login attempt");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", "An internal server error occured");
                HelperSerilog.LogError(ex.Message, ex);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Tracking");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RegistrationError", "An error occured during signing up");
                HelperSerilog.LogError(ex.Message, ex);
            }

            return View(model);
        }
    }
}
