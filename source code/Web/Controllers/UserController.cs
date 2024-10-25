using Microsoft.AspNetCore.Mvc;
using Web.Models.Business_Entities;

namespace Web.Controllers
{
    public class UserController : BaseController
    {

        public UserController()
        {

        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Login");
        }
    }
}
