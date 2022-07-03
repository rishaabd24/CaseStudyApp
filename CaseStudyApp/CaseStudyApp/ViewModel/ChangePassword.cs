using System.ComponentModel.DataAnnotations;

namespace CaseStudyApp.ViewModel
{
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPwd { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPwd { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPwd", ErrorMessage = "Passwords dont match")]
        public string ConfirmPwd { get; set; }
    }
}
