using CaseStudyApp.Data;
using CaseStudyApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyApp.Model
{
    public class AuthDbContext:IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<FileOnFileSystemModel> FilesOnFileSystem { get; set; }
    }

}
