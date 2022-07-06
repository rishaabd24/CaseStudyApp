using Microsoft.AspNetCore.Identity;

namespace CaseStudyApp.Data
{
    //Model for a user which inherits IdentityUser
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayUsername { get; set; }
    }
}
