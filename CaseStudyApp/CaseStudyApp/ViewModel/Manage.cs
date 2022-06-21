using System.ComponentModel.DataAnnotations;

namespace CaseStudyApp.ViewModel
{
    public class Manage
    {
        public string Id { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
