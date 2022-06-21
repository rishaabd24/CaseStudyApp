using CaseStudyApp.Data;
using CaseStudyApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CaseStudyApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        [BindProperty]
        public Register Model { get; set; }
        public RegisterModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = Model.Email, //one change here
                    Email = Model.Email,
                    PhoneNumber = Model.PhoneNo,
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    DisplayUsername = Model.Username,
                };
                var result = await userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
