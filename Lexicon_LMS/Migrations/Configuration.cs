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
            // Create courses
            Course[] courses = new[]
            {
                new Course{ Name = "DNMVC17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,15) ,Description = "Fusce mattis maximus maximus. Ut eu facilisis ipsum. Phasellus tincidunt ut diam non malesuada. Vestibulum facilisis pharetra purus. Cras viverra posuere mattis. Vestibulum dui purus, rhoncus ut consectetur non, rhoncus at justo. Quisque id maximus est, et ornare risus. Donec nec justo sed ex euismod commodo. Quisque tempus, est laoreet commodo dictum, ipsum nulla egestas lorem, id tempor elit nisl non nisl. Donec vel urna vitae felis consectetur laoreet." },
                new Course{ Name = "DJANGO17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,29) ,Description = "Maecenas non convallis est. Quisque varius interdum tempor. Phasellus at erat ornare, sagittis ligula eu, cursus nibh. Mauris ac quam ut est interdum facilisis. Aliquam eget fermentum diam. Nam orci augue, fringilla quis maximus at, eleifend at urna. Aliquam erat volutpat. Nam quis mauris et nisi ornare consectetur. Duis ac urna vitae odio gravida venenatis. Aenean sed elit luctus, dictum turpis sit amet, tristique enim. Nunc ac augue accumsan, mollis orci at, molestie dui. Praesent dapibus dictum velit, id mattis diam tempor nec." },
            };
            context.Courses.AddOrUpdate(x => x.Name, courses);
            context.SaveChanges();

            //

            // Create user roles
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { Role.Teacher, Role.Student };
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;

                var result = roleManager.Create(new IdentityRole { Name = roleName });

                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors));
            }

            // Create users
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var users = new[] { new {email="foo@bar.com", first="foo", last="bar", role = Role.Teacher, course = (string) null}, //TODO check how to declare null strings
                                new {email="student1@foo.com", first="Adam", last="Adamsson", role = Role.Student, course = "DNMVC17"},
                                new {email="student2@foo.com", first="Bert", last="Bertsson", role = Role.Student, course = "DNMVC17"},
                                new {email="student3@foo.com", first="Fredrik", last="Fredriksson", role = Role.Student, course = "DNMVC17"},
                                new {email="student4@foo.com", first="Gustav", last="Gustavsson", role = Role.Student, course = "DJANGO17"}};

            foreach (var user in users)
            {
                if (context.Users.Any(u => u.UserName == user.email)) continue;

                var newUser = new ApplicationUser { UserName = user.email, Email = user.email, FirstName = user.first, LastName = user.last };
                var result = userManager.Create(newUser, "foobar");

                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors));
            }

            // Add references to users
            foreach (var user in users)
            {
                if (user.course != null)
                    userManager.FindByName(user.email).Course = context.Courses.First(x => x.Name == user.course);
                userManager.AddToRole(userManager.FindByName(user.email).Id, user.role);
            }
        }
    }
}
