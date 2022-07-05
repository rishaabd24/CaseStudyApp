using CaseStudyApp.Data;
using CaseStudyApp.Model;
using CaseStudyApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseStudyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly UserManager<AppUser> signInManager;
        public AccountController(AuthDbContext context, UserManager<AppUser> userManager, UserManager<AppUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager= signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }
    }
}
