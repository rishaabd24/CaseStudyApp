using CaseStudyApp.Data;
using CaseStudyApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CaseStudyApp.Pages
{
    public class ManageModel : PageModel
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        [BindProperty]
        public Manage Model { get; set; }
        public string StatusMessage { get; set; }

        public ManageModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public string Username;
        public async Task LoadAsync(AppUser user)
        {
            var userName = await userManager.GetUserNameAsync(user);
            var phone = await userManager.GetPhoneNumberAsync(user);
            Username = userName;
            var Model = new Manage
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.DisplayUsername,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = phone
            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound($"Unable to load User with ID '{userManager.GetUserId(User)}'");
            }
            await LoadAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load User with ID '{userManager.GetUserId(User)}'");
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var phoneNo = await userManager.GetPhoneNumberAsync(user);
            if(Model.Phone != phoneNo)
            {
                if (Model.Phone != null)
                {
                    var setPhone = await userManager.SetPhoneNumberAsync(user, Model.Phone);
                    if (!setPhone.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set phone number.";
                        return RedirectToPage();
                    }
                }
            }
            if(Model.FirstName != user.FirstName)
            {
                if (Model.FirstName != null)
                {
                    user.FirstName = Model.FirstName;
                }
            }
            if(Model.LastName != user.LastName)
            {
                if (Model.LastName != null)
                {
                    user.LastName = Model.LastName;
                }
            }
            if(Model.Username != user.DisplayUsername)
            {
                if(Model.Username != null)
                {
                    user.DisplayUsername = Model.Username;
                }
            }
            if(Model.Email != user.Email)
            {
                if (Model.Email != null)
                {
                    user.Email = Model.Email;
                    user.UserName = Model.Email;
                }
            }
            var result = await userManager.UpdateAsync(user);
            await signInManager.RefreshSignInAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //return RedirectToPage();
            }
            else
            {
                StatusMessage = "User details updated";
            }
            return RedirectToPage();
        }
    }
}
