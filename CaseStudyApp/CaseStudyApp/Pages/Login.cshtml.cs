using CaseStudyApp.Data;
using CaseStudyApp.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CaseStudyApp.Pages
{
    public class LoginModel : PageModel
    {
        [TempData]
        public string ErrorMsg { get; set; }
        public string ReturnUrl { get; set; }

        private readonly SignInManager<AppUser> signInManager;
        [BindProperty]
        public Login Model { get; set; }
        public LoginModel(SignInManager<AppUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                ModelState.AddModelError(string.Empty, ErrorMsg);
            }
            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe,lockoutOnFailure:false); //one change here
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
                    ModelState.AddModelError(string.Empty, "Username or password invalid");
                    return Page();
                }
            }
            return Page();
        }
    }
}
