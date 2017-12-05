using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        // GET: 
        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateModule(int? courseId, string returnUrl = "/")
        {
            if (courseId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // get latest end-date from module list OR the course start-date
            var startDate = db.Courses.FirstOrDefault(c => c.Id == courseId)
                .Modules.Select(m => m.EndDate)
                .Concat(
                    db.Courses.Where(c => c.Id == courseId)
                    .Select(c => c.StartDate))
                .Max().AddDays(1);

            var module = new Module
            {
                CourseId = (int)courseId,
                StartDate = startDate,
                EndDate = startDate
            };

            ViewBag.returnUrl = returnUrl;
            return View(module);
        }

        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule(Module module, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToLocal(returnUrl);
            }
            ViewBag.returnUrl = returnUrl;
            return View(module);
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult EditModule(int? id, string returnUrl = "/")
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);
            if (module == null)
                return HttpNotFound();

            ViewBag.returnUrl = returnUrl;
            return View(module);
        }

        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToLocal(returnUrl);
            }
            return View(module);
        }

        public ActionResult ListActivity(int id)
        {
            var activity = db.Activities.Where(m => m.ModuleId == id).ToList();

            var module = db.Modules.Find(id);

            return PartialView(module);
        }
        // GET: 
        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateActivity(int? id, string returnUrl = "/")
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // get latest end-date from module list OR the course start-date
            var startDate = db.Modules.FirstOrDefault(c => c.Id == id)
                .Activity.Select(m => m.EndDate)
                .Concat(
                    db.Modules.Where(c => c.Id == id)
                    .Select(c => c.StartDate))
                .Max().AddDays(1);

            var activity = new Activity
            {
                ModuleId = (int)id,
                ActivityTypes = db.ActivityTypes.ToList(),
                StartDate = startDate,
                EndDate = startDate
            };

            ViewBag.returnUrl = returnUrl;
            return View(activity);
        }

        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(Activity activity, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToLocal(returnUrl);
            }
            ViewBag.returnUrl = returnUrl;
            activity.ActivityTypes = db.ActivityTypes.ToList();

            return View(activity);
        }


        //TODO copy from accountcontroller ... move to common utility class
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }



    }
}