using System.ComponentModel.DataAnnotations;

namespace CaseStudyApp.ViewModel
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; } //added a specific username field in Register as shown in the SS in the docx
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password),ErrorMessage ="Passwords don't match")]
        public string ConfirmPassword { get; set; }
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
