using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.Helpers;
using Web.Models.Business_Entities;
using Web.Models.General_Entities;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Remote validation
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsUsernameInUse(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user != null)
            {
                return Json($"Username {username} is already in use");
            }
            else
            {
                return Json(true);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return Json($"Email {email} is already in use");
            }
            else
            {
                return Json(true);
            }
        }

        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl = null)
        {
            var model = new LoginViewModel();

            if (User.Identity?.IsAuthenticated == true)
            {
                if (!ReturnUrl.IsNullOrEmpty() && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl!);
                }
                else
                {
                    return RedirectToAction("Index", "Tracking");
                }
            }

            model.ReturnUrl = ReturnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
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
                    if (!model.ReturnUrl.IsNullOrEmpty() && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl!);
                    }
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = new User { UserName = model.Username, Email = model.Email };
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

        [HttpGet]
        public IActionResult Role()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Role(CreateRole model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityRole role = new IdentityRole() { Name = model.Name };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Tracking", new { IsSaved = true, message = "Role created successfully" });
            }
            else
            {
                return RedirectToAction("Index", "Tracking", new { IsSaved = false, message = "Failed to create role" });
            }
        }
    }


}
