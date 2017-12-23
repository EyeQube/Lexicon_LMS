namespace Lexicon_LMS.Migration
{
    using Lexicon_LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationTwo : DbMigrationsConfiguration<Lexicon_LMS.Models.ApplicationDbContext>
    {
        public ConfigurationTwo()
        {
            AutomaticMigrationsEnabled = false;
        }
            
        protected override void Seed(ApplicationDbContext context)
        {
            // Create courses
            Course[] courses = new[]
            {
                new Course{ Name = "DNMVC17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,15,23,59,59) ,Description = "Välkommen till kursportalen för klass ND17.\nHär hittar ni under utbildningens gång scheman, material och uppgiftningsbeskrivningar." },
                new Course{ Name = "DJANGO17", StartDate = new DateTime(2017,11,27), EndDate = new DateTime(2017,12,29,23,59,59) ,Description = "Maecenas non convallis est. Quisque varius interdum tempor. Phasellus at erat ornare, sagittis ligula eu, cursus nibh. Mauris ac quam ut est interdum facilisis. Aliquam eget fermentum diam. Nam orci augue, fringilla quis maximus at, eleifend at urna. Aliquam erat volutpat. Nam quis mauris et nisi ornare consectetur. Duis ac urna vitae odio gravida venenatis. Aenean sed elit luctus, dictum turpis sit amet, tristique enim. Nunc ac augue accumsan, mollis orci at, molestie dui. Praesent dapibus dictum velit, id mattis diam tempor nec." },
            };
            context.Courses.AddOrUpdate(x => x.Name, courses);
            context.SaveChanges();

            // Create modules 
            var dnCourseId = context.Courses.First(x => x.Name == "DNMVC17").Id;
            Module[] dnModules = new[]
            {
                new Module{ CourseId = dnCourseId, Name = "C#", StartDate = new DateTime(2017,08,28), EndDate = new DateTime(2017,09,26,23,59,59), Description = "Grundutbildning C#, objektorienterad design och LINQ-biblioteket"},
                new Module{ CourseId = dnCourseId, Name = "Webb", StartDate = new DateTime(2017,09,27), EndDate = new DateTime(2017,10,18,23,59,59), Description = "Grundutbildningi HTML5, CSS och Bootstrap samt Git"},
                new Module{ CourseId = dnCourseId, Name = "MVC", StartDate = new DateTime(2017,10,11), EndDate = new DateTime(2017,10,26,23,59,59), Description = "Microsofts MVC-ramverk"},
                new Module{ CourseId = dnCourseId, Name = "Databas", StartDate = new DateTime(2017,10,27), EndDate = new DateTime(2017,11,06,23,59,59), Description = "Relationsdatabaser och EntityFramework"},
                new Module{ CourseId = dnCourseId, Name = "App.Utv.", StartDate = new DateTime(2017,11,07), EndDate = new DateTime(2017,11,15,23,59,59), Description = "Applikationsutveckling med jQuery, AJAX och UX-design"},
                new Module{ CourseId = dnCourseId, Name = "MVC fördjupning", StartDate = new DateTime(2017,11,14), EndDate = new DateTime(2017,12,15,23,59,59), Description = "Grupparbete enligt scrum-metodiken"},
            };
            context.Modules.AddOrUpdate(x => x.Name, dnModules);
            context.SaveChanges();

            //Create ActivityType
            ActivityType[] activityTypes = new[]
            {
                new ActivityType{ Name = "Föreläsning", AllowStudentDocuments = false},
                new ActivityType{ Name = "E-learning" , AllowStudentDocuments = false},
                new ActivityType{ Name = "Övning" , AllowStudentDocuments = true},
                new ActivityType{ Name = "övrig" , AllowStudentDocuments = false}
            };
            context.ActivityTypes.AddOrUpdate(x => x.Name, activityTypes);
            context.SaveChanges();

            //Create Activity
            var ModCsh = context.Modules.First(x => x.Name == "C#").Id;
            var ModWeb = context.Modules.First(x => x.Name == "Webb").Id;
            var ModMVC = context.Modules.First(x => x.Name == "MVC").Id;
            var ModDat = context.Modules.First(x => x.Name == "Databas").Id;
            var ModApp = context.Modules.First(x => x.Name == "App.Utv.").Id;
            var ModPro = context.Modules.First(x => x.Name == "MVC fördjupning").Id;

            Activity[] dnActivity = new[]
            {
new Activity { ModuleId = ModCsh, Name="Intro + E-L 1.1, 1.2", StartDate = new DateTime(2017,08,28,13,00,00), EndDate = new DateTime(2017,08,28,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="E-L 1.3 ", StartDate = new DateTime(2017,08,29,08,30,00), EndDate = new DateTime(2017,08,29,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="E-L 1.4 + 1.5", StartDate = new DateTime(2017,08,29,13,00,00), EndDate = new DateTime(2017,08,29,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="Frl C# Intro", StartDate = new DateTime(2017,08,30,08,30,00), EndDate = new DateTime(2017,08,30,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="Övning 2", StartDate = new DateTime(2017,08,31,08,30,00), EndDate = new DateTime(2017,08,31,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModCsh, Name="Frl C# Grund", StartDate = new DateTime(2017,09,01,08,30,00), EndDate = new DateTime(2017,09,01,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="E-L 1.6 + 1.7", StartDate = new DateTime(2017,09,04,08,30,00), EndDate = new DateTime(2017,09,04,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="E-L 1.8", StartDate = new DateTime(2017,09,04,13,00,00), EndDate = new DateTime(2017,09,04,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="E-L 1.7 + 1.8", StartDate = new DateTime(2017,09,05,08,30,00), EndDate = new DateTime(2017,09,05,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="Övn 2 / Repetition", StartDate = new DateTime(2017,09,05,13,00,00), EndDate = new DateTime(2017,09,05,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModCsh, Name="FRL: OOP", StartDate = new DateTime(2017,09,06,08,30,00), EndDate = new DateTime(2017,09,06,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="Övning 3", StartDate = new DateTime(2017,09,07,08,30,00), EndDate = new DateTime(2017,09,11,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModCsh, Name="FRL OOP 2", StartDate = new DateTime(2017,09,08,08,30,00), EndDate = new DateTime(2017,09,08,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="E-L 2.1 - 2.4", StartDate = new DateTime(2017,09,12,08,30,00), EndDate = new DateTime(2017,09,12,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="Övning 4", StartDate = new DateTime(2017,09,12,13,00,00), EndDate = new DateTime(2017,09,15,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModCsh, Name="FRL Generics", StartDate = new DateTime(2017,09,13,08,30,00), EndDate = new DateTime(2017,09,13,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="E-L 2.5 - 2.6", StartDate = new DateTime(2017,09,14,08,30,00), EndDate = new DateTime(2017,09,14,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="E-L 2.7 - 2.9", StartDate = new DateTime(2017,09,15,08,30,00), EndDate = new DateTime(2017,09,15,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="FRL LINQ", StartDate = new DateTime(2017,09,18,13,00,00), EndDate = new DateTime(2017,09,18,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="Unit Test E-L Test", StartDate = new DateTime(2017,09,19,08,30,00), EndDate = new DateTime(2017,09,19,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModCsh, Name="FRL/Ariktektur", StartDate = new DateTime(2017,09,20,08,30,00), EndDate = new DateTime(2017,09,20,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="FRL/Test", StartDate = new DateTime(2017,09,20,13,00,00), EndDate = new DateTime(2017,09,20,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModCsh, Name="Övning Garage 1.0", StartDate = new DateTime(2017,09,21,08,30,00), EndDate = new DateTime(2017,09,26,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModCsh, Name="Redovisning", StartDate = new DateTime(2017,09,26,13,00,00), EndDate = new DateTime(2017,09,26,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModWeb, Name="E-L: 3 + 4.1-4.3", StartDate = new DateTime(2017,09,27,08,30,00), EndDate = new DateTime(2017,09,27,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="E-L: 4.4-4.5", StartDate = new DateTime(2017,09,28,08,30,00), EndDate = new DateTime(2017,09,28,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="Övning CSS", StartDate = new DateTime(2017,09,28,13,00,00), EndDate = new DateTime(2017,09,28,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModWeb, Name="Frl Html/Css", StartDate = new DateTime(2017,09,29,08,30,00), EndDate = new DateTime(2017,09,29,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModWeb, Name="JavaScript CodeSchool", StartDate = new DateTime(2017,10,02,08,30,00), EndDate = new DateTime(2017,10,02,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModWeb, Name="E-L: 5.1 - 5.3", StartDate = new DateTime(2017,10,03,08,30,00), EndDate = new DateTime(2017,10,03,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="E-L: 5.3 - 5.5", StartDate = new DateTime(2017,10,03,13,00,00), EndDate = new DateTime(2017,10,03,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="FRL JS", StartDate = new DateTime(2017,10,04,08,30,00), EndDate = new DateTime(2017,10,04,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModWeb, Name="FRL/Övning JS", StartDate = new DateTime(2017,10,04,13,00,00), EndDate = new DateTime(2017,10,04,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModWeb, Name="E-L/Övning JS", StartDate = new DateTime(2017,10,05,08,30,00), EndDate = new DateTime(2017,10,05,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="E-L 6 Bootstrap", StartDate = new DateTime(2017,10,06,08,30,00), EndDate = new DateTime(2017,10,06,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="E-L/Övning Bootstrap", StartDate = new DateTime(2017,10,09,08,30,00), EndDate = new DateTime(2017,10,09,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModWeb, Name="Övning Bootstrap", StartDate = new DateTime(2017,10,09,13,00,00), EndDate = new DateTime(2017,10,10,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModMVC, Name="FRL: ASP.NET MVC", StartDate = new DateTime(2017,10,11,08,30,00), EndDate = new DateTime(2017,10,11,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModMVC, Name="E-L 7.1 - 7.3", StartDate = new DateTime(2017,10,12,08,30,00), EndDate = new DateTime(2017,10,12,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModMVC, Name="FRL MVC", StartDate = new DateTime(2017,10,13,08,30,00), EndDate = new DateTime(2017,10,13,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModMVC, Name="E-L 7.4", StartDate = new DateTime(2017,10,16,08,30,00), EndDate = new DateTime(2017,10,16,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModMVC, Name="E-L 7.5 + Övn MVC", StartDate = new DateTime(2017,10,16,13,00,00), EndDate = new DateTime(2017,10,16,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModMVC, Name="E-L Repetition + Övn", StartDate = new DateTime(2017,10,17,08,30,00), EndDate = new DateTime(2017,10,19,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModMVC, Name="7.6 + Övn", StartDate = new DateTime(2017,10,17,13,00,00), EndDate = new DateTime(2017,10,19,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModWeb, Name="FRL: Git", StartDate = new DateTime(2017,10,18,08,30,00), EndDate = new DateTime(2017,10,18,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModMVC, Name="FRL ViewModel", StartDate = new DateTime(2017,10,20,08,30,00), EndDate = new DateTime(2017,10,20,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModMVC, Name="FRL PartialView", StartDate = new DateTime(2017,10,20,13,00,00), EndDate = new DateTime(2017,10,20,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModMVC, Name="Övning Garage 2.0", StartDate = new DateTime(2017,10,23,08,30,00), EndDate = new DateTime(2017,10,26,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModMVC, Name="Redovisning", StartDate = new DateTime(2017,10,26,13,00,00), EndDate = new DateTime(2017,10,26,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModDat, Name="Frl Datamodellering", StartDate = new DateTime(2017,10,27,08,30,00), EndDate = new DateTime(2017,10,27,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModDat, Name="Övning 13", StartDate = new DateTime(2017,10,27,13,00,00), EndDate = new DateTime(2017,10,27,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModDat, Name="Frl EntityFramework", StartDate = new DateTime(2017,10,30,08,30,00), EndDate = new DateTime(2017,10,30,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModDat, Name="SQLBolt.com", StartDate = new DateTime(2017,10,31,08,30,00), EndDate = new DateTime(2017,10,31,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModDat, Name="Garage 2.5", StartDate = new DateTime(2017,11,02,08,30,00), EndDate = new DateTime(2017,11,06,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModDat, Name="Halv dag", StartDate = new DateTime(2017,11,03,13,00,00), EndDate = new DateTime(2017,11,03,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModApp, Name="Garage 2.5", StartDate = new DateTime(2017,11,07,08,30,00), EndDate = new DateTime(2017,11,07,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModApp, Name="E-L 10 / Övn UX", StartDate = new DateTime(2017,11,07,13,00,00), EndDate = new DateTime(2017,11,07,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModApp, Name="FRL UX", StartDate = new DateTime(2017,11,08,08,30,00), EndDate = new DateTime(2017,11,08,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModApp, Name="FRL UX / Övning 16", StartDate = new DateTime(2017,11,08,13,00,00), EndDate = new DateTime(2017,11,08,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModApp, Name="Jquery CodeSchool", StartDate = new DateTime(2017,11,09,08,30,00), EndDate = new DateTime(2017,11,09,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModApp, Name="Frl Jquery/Ajax", StartDate = new DateTime(2017,11,10,08,30,00), EndDate = new DateTime(2017,11,10,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModApp, Name="Identity FRL", StartDate = new DateTime(2017,11,13,08,30,00), EndDate = new DateTime(2017,11,13,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModPro, Name="E-L 12 MVC", StartDate = new DateTime(2017,11,14,08,30,00), EndDate = new DateTime(2017,11,16,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 2 },
new Activity { ModuleId = ModPro, Name="Övning 17", StartDate = new DateTime(2017,11,14,13,00,00), EndDate = new DateTime(2017,11,16,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 3 },
new Activity { ModuleId = ModApp, Name="Frl Client vs. Server", StartDate = new DateTime(2017,11,15,08,30,00), EndDate = new DateTime(2017,11,15,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModPro, Name="SCRUM FRL", StartDate = new DateTime(2017,11,17,08,30,00), EndDate = new DateTime(2017,11,17,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 1 },
new Activity { ModuleId = ModPro, Name="Projektplanering", StartDate = new DateTime(2017,11,20,08,30,00), EndDate = new DateTime(2017,11,21,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Projektstart", StartDate = new DateTime(2017,11,22,08,30,00), EndDate = new DateTime(2017,11,22,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Planering sprint 1", StartDate = new DateTime(2017,11,22,13,00,00), EndDate = new DateTime(2017,11,22,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Sprint review", StartDate = new DateTime(2017,11,29,08,30,00), EndDate = new DateTime(2017,12,13,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Planering sprint 2", StartDate = new DateTime(2017,11,29,13,00,00), EndDate = new DateTime(2017,11,29,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Planering sprint 3", StartDate = new DateTime(2017,12,06,13,00,00), EndDate = new DateTime(2017,12,06,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Slutredovisning", StartDate = new DateTime(2017,12,15,08,30,00), EndDate = new DateTime(2017,12,15,12,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },
new Activity { ModuleId = ModPro, Name="Avslutning", StartDate = new DateTime(2017,12,15,13,00,00), EndDate = new DateTime(2017,12,15,17,00,00), Description = "(placeholder description text)", ActivityTypeId = 4 },

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