using CaseStudyApp.Data;
using CaseStudyApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CaseStudyApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> signInManager;
        [BindProperty]
        public Login Model { get; set; }
        public LoginModel(SignInManager<AppUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false); //one change here
                if (identityResult.Succeeded)
                {
                    if(returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        return RedirectToPage("returnUrl");
                    }
                }
                else
                {
                    ModelState.AddModelError("LogOnError", "The Username or Password is incorrect. Please try again!");

                }
            }
            return Page();
        }
    }
}
