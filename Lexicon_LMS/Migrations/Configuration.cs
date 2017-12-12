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
                new Course{ Name = "DNMVC17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,15) ,Description = "Välkommen till kursportalen för klass ND17.\nHär hittar ni under utbildningens gång scheman, material och uppgiftningsbeskrivningar." },
                new Course{ Name = "DJANGO17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,29) ,Description = "Maecenas non convallis est. Quisque varius interdum tempor. Phasellus at erat ornare, sagittis ligula eu, cursus nibh. Mauris ac quam ut est interdum facilisis. Aliquam eget fermentum diam. Nam orci augue, fringilla quis maximus at, eleifend at urna. Aliquam erat volutpat. Nam quis mauris et nisi ornare consectetur. Duis ac urna vitae odio gravida venenatis. Aenean sed elit luctus, dictum turpis sit amet, tristique enim. Nunc ac augue accumsan, mollis orci at, molestie dui. Praesent dapibus dictum velit, id mattis diam tempor nec." },
            };
            context.Courses.AddOrUpdate(x => x.Name, courses);
            context.SaveChanges();

            // Create modules 
            var dnCourseId = context.Courses.First(x => x.Name == "DNMVC17").Id;
            Module[] dnModules = new[]
            {
                new Module{ CourseId = dnCourseId, Name = "C#", StartDate = new DateTime(2017,08,28), EndDate = new DateTime(2017,09,26), Description = "Grundutbildning C#, objektorienterad design och LINQ-biblioteket"},
                new Module{ CourseId = dnCourseId, Name = "Webb", StartDate = new DateTime(2017,09,27), EndDate = new DateTime(2017,10,18), Description = "Grundutbildningi HTML5, CSS och Bootstrap samt Git"},
                new Module{ CourseId = dnCourseId, Name = "MVC", StartDate = new DateTime(2017,10,11), EndDate = new DateTime(2017,10,26), Description = "Microsofts MVC-ramverk"},
                new Module{ CourseId = dnCourseId, Name = "Databas", StartDate = new DateTime(2017,10,27), EndDate = new DateTime(2017,11,06), Description = "Relationsdatabaser och EntityFramework"},
                new Module{ CourseId = dnCourseId, Name = "App.Utv.", StartDate = new DateTime(2017,11,07), EndDate = new DateTime(2017,11,15), Description = "Applikationsutveckling med jQuery, AJAX och UX-design"},
                new Module{ CourseId = dnCourseId, Name = "MVC fördjupning", StartDate = new DateTime(2017,11,14), EndDate = new DateTime(2017,12,15), Description = "Grupparbete enligt scrum-metodiken"},
            };
            context.Modules.AddOrUpdate(x => x.Name, dnModules);
            context.SaveChanges();

            //Create ActivityType
            ActivityType[] activityTypes = new[]
            {
                new ActivityType{ Name = "Föreläsning", AllowStudentDocuments = false},
                new ActivityType{ Name = "E-learning" , AllowStudentDocuments = false},
                new ActivityType{ Name = "Övning" , AllowStudentDocuments = true}
            };
            context.ActivityTypes.AddOrUpdate(x => x.Name, activityTypes);
            context.SaveChanges();

            //Create Activity
            var mod1Id = context.Modules.First(x => x.Name == "C#").Id;
            var mod2Id = context.Modules.First(x => x.Name == "Webb").Id;
            var mod3Id = context.Modules.First(x => x.Name == "MVC").Id;
            var mod4Id = context.Modules.First(x => x.Name == "Databas").Id;
            var mod5Id = context.Modules.First(x => x.Name == "App.Utv").Id;
            var mod6Id = context.Modules.First(x => x.Name == "MVC fördjupning").Id;
            Activity[] dnActivity = new[]
            {
                new Activity { ModuleId = mod1Id, Name= "El-1.1 to 1.9", StartDate = new DateTime(2017,11,27,08,00,00), EndDate = new DateTime(2017,11,27,12,00,00), Description = "Scott Allan Basic C#", ActivityTypeId = 2 },
                new Activity { ModuleId = mod1Id, Name= "Övning 1", StartDate = new DateTime(2017,11,27,13,00,00), EndDate = new DateTime(2017,11,27,17,00,00), Description = "Göra en for loop", ActivityTypeId = 3 },
                new Activity { ModuleId = mod1Id, Name= "Frl C# Grund", StartDate = new DateTime(2017,11,28,08,00,00), EndDate = new DateTime(2017,11,28,17,00,00), Description = "Gå igenom grunderna i C#", ActivityTypeId = 1 }
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
