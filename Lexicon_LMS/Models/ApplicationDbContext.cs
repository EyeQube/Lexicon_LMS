using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Lexicon_LMS.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private string v;

        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        //public DbSet<Event> Events { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public ApplicationDbContext(string v)
        {
            this.v = v;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}