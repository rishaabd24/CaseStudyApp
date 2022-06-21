using System.ComponentModel.DataAnnotations;

namespace CaseStudyApp.ViewModel
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; } //added a specific username field in Register as shown in the SS in the docx
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Passwords don't match")]
        public string ConfirmPassword { get; set; }
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
