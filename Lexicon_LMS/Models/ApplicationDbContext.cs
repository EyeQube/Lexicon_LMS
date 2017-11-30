using Lexicon_LMS.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Lexicon_LMS.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleFormViewModel> CourseModule { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}