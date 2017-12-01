using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Lexicon_LMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }


        [Authorize] 
        public ActionResult Module(int id)
        {
            var dbCourse = db.Courses.Single(c => c.Id == id);

            // get latest end-date from module list OR the course start-date
            
            var startDate = db.Courses.FirstOrDefault(c => c.Id == id)
                .Modules.Select(m => m.EndDate)
                .Concat(
                    db.Courses.Where(c => c.Id == id)
                    .Select(c => c.StartDate))
                .Max();


            Module module = new Module()
            {
                CourseId = dbCourse.Id,
                StartDate = startDate,
                EndDate = startDate
            };

            return View(module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveModule(Module module)  
        {

            if (module.Id == 0)
            {
                db.Modules.Add(module);
            }
            else
            {
                var moduleInDb = db.Modules.Single(m => m.Id == module.Id);
                moduleInDb.Name = module.Name;
                moduleInDb.Description = module.Description;
                moduleInDb.StartDate = module.StartDate;
                moduleInDb.EndDate = module.EndDate;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }  
           

        [Authorize]
        public ActionResult Course(int id)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
                return HttpNotFound();

            if (User.IsInRole(Role.Student))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user.CourseId != course.Id)
                {
                    ViewBag.ErrorMessage = $"You have no access to course ({course.Name})";
                    return View("Error");
                }
            }
            return View(course);
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Register()
        {
            var Course = new Course();

            return View("RegisterCourse", Course);
        }

        //
        // POST: /Home/SaveCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveCourse(Course course)
        {

            if (course.Id == 0)
            {
                db.Courses.Add(course);
            }
            else
            {
                var courseInDb = db.Courses.Single(c => c.Id == course.Id);
                courseInDb.Name = course.Name;
                courseInDb.Description = course.Description;
                courseInDb.StartDate = course.StartDate;
                courseInDb.EndDate = course.EndDate;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListCourses()
        {
            var Courses = db.Courses.ToList();

            return View(Courses);
        }

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole(Role.Teacher))
                return RedirectToAction("ListCourses");

            var user = db.Users.Find(User.Identity.GetUserId());
            var courseid = user?.Course?.Id;

            if (courseid != null)
                return RedirectToAction("Course", new { Id = courseid });

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}