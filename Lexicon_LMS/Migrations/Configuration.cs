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
                new Course{ Name = "DNMVC17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,15,23,59,59) ,Description = "Fusce mattis maximus maximus. Ut eu facilisis ipsum. Phasellus tincidunt ut diam non malesuada. Vestibulum facilisis pharetra purus. Cras viverra posuere mattis. Vestibulum dui purus, rhoncus ut consectetur non, rhoncus at justo. Quisque id maximus est, et ornare risus. Donec nec justo sed ex euismod commodo. Quisque tempus, est laoreet commodo dictum, ipsum nulla egestas lorem, id tempor elit nisl non nisl. Donec vel urna vitae felis consectetur laoreet." },
                new Course{ Name = "DJANGO17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,29,23,59,59) ,Description = "Maecenas non convallis est. Quisque varius interdum tempor. Phasellus at erat ornare, sagittis ligula eu, cursus nibh. Mauris ac quam ut est interdum facilisis. Aliquam eget fermentum diam. Nam orci augue, fringilla quis maximus at, eleifend at urna. Aliquam erat volutpat. Nam quis mauris et nisi ornare consectetur. Duis ac urna vitae odio gravida venenatis. Aenean sed elit luctus, dictum turpis sit amet, tristique enim. Nunc ac augue accumsan, mollis orci at, molestie dui. Praesent dapibus dictum velit, id mattis diam tempor nec." },
            };
            context.Courses.AddOrUpdate(x => x.Name, courses);
            context.SaveChanges();

            // Create modules 
            var dnCourseId = context.Courses.First(x => x.Name == "DNMVC17").Id;
            Module[] dnModules = new[]
            {
                new Module{ CourseId = dnCourseId, Name = "C# grundkurs", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,5,23,59,59), Description = "Grunderna i C#"},
                new Module{ CourseId = dnCourseId, Name = "C# OO", StartDate = new DateTime(2017,12,6), EndDate = new DateTime(2017,12,10,23,59,59), Description = "Objekt orienterad mjukvaruutveckling i teori och praktik"},
                new Module{ CourseId = dnCourseId, Name = "C# LINQ", StartDate = new DateTime(2017,12,10), EndDate = new DateTime(2017,12,15,23,59,59), Description = "Introduktion till biblioteket LINQ"},
            };
            context.Modules.AddOrUpdate(x => x.Name, dnModules);
            context.SaveChanges();

            //Create ActivityType
            ActivityType[] activityTypes = new[]
            {
                new ActivityType{ Name = "Föreläsning", AllowStudentDocuments = false},
                new ActivityType{ Name = "E-learning" , AllowStudentDocuments = false},
                new ActivityType{ Name = "Övningstillfälle" , AllowStudentDocuments = true}
            };
            context.ActivityTypes.AddOrUpdate(x => x.Name, activityTypes);
            context.SaveChanges();

            //Create Activity
            var dnModuleId = context.Modules.First(x => x.Name == "C# grundkurs").Id;
            Activity[] dnActivity = new[]
            {
                new Activity { ModuleId = dnModuleId, Name= "El-1.1 to 1.9", StartDate = new DateTime(2017,11,27,08,00,00), EndDate = new DateTime(2017,11,27,12,00,00), Description = "Scott Allan Basic C#", ActivityTypeId = 2 },
                new Activity { ModuleId = dnModuleId, Name= "Övning 1", StartDate = new DateTime(2017,11,27,13,00,00), EndDate = new DateTime(2017,11,27,17,00,00), Description = "Göra en for loop", ActivityTypeId = 3 },
                new Activity { ModuleId = dnModuleId, Name= "Frl C# Grund", StartDate = new DateTime(2017,11,28,08,00,00), EndDate = new DateTime(2017,11,28,17,00,00), Description = "Gå igenom grunderna i C#", ActivityTypeId = 1 }
            };
            context.Activities.AddOrUpdate(x => x.Name, dnActivity);
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
