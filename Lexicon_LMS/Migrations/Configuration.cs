namespace Lexicon_LMS.Migrations
{
    using Lexicon_LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Lexicon_LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { Role.Teacher, Role.Student };
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;

                //Create role
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //var emails = new[] { "Teacher@lexicon.se", "Student1@lexicon.se", "Student2@lexicon.se", "Student3@lexicon.se" };

            var users = new[] { new {email="foo@bar.com", first="foo", last="bar"},
                                new {email="bar@foo.com", first="bar", last="foo"}};

            foreach (var user in users)
            {
                if (context.Users.Any(u => u.UserName == user.email)) continue;

                //Create user
                var userUser = new ApplicationUser { UserName = user.email, Email = user.email, FirstName = user.first, LastName = user.last };
                var result = userManager.Create(userUser, "foobar");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var teacherUser = userManager.FindByName("foo@bar.com");
            userManager.AddToRole(teacherUser.Id, "Teacher");

            var studentUser = userManager.FindByName("bar@foo.com");
            userManager.AddToRole(studentUser.Id, "Student");
        }
    }
}
