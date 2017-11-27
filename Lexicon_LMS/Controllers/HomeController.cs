using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
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
        
        public ActionResult Course(int Id)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == Id);

            var u = course.Users.ToList();
            if (course != null)
                return View(course);
            else
                return HttpNotFound();
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Register(string option)
        {

            if (option == "course")
            {
                var Course = new Course();

                return View("RegisterCourse", Course);
            }

            if (option == "role")
            {
                var viewModel = new RegisterViewModel
                {
                    Roles = Role.Student
                };

                //var rolen = _context.Roles.Select(s => s.Name).ToList();  
                return View("Register", viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Home/SaveCourse
        [HttpPost]
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
                courseInDb.StartDate = course.StartDate;
                courseInDb.EndDate = course.EndDate;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var courseid = user?.Course?.Id ?? 0;

                if (courseid != 0)
                    return RedirectToAction("Course", new { Id = courseid });
                else
                    return View();
            }



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