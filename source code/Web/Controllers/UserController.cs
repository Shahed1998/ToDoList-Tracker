using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Login");
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
            catch(Exception ex)
            {
                ModelState.AddModelError("RegistrationError", "An error occured during signing up");
                HelperSerilog.LogError(ex.Message, ex);
            }

            return View(model);
        }
    }
}
