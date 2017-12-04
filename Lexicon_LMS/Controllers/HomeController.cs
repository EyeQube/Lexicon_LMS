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
        public ActionResult Module(int id)
        {

            // get latest end-date from module list OR the course start-date

            var dbCourse = db.Courses.Single(c => c.Id == id);

            Module module = new Module();

            if (dbCourse.Modules.Count() == 0)
            {
                module.CourseId = dbCourse.Id;
                module.StartDate = dbCourse.StartDate;
                module.EndDate = dbCourse.EndDate;

                /*var _startDate = db.Courses.FirstOrDefault(c => c.Id == id);
                startDate = _startDate.StartDate; */

                return View(module);

                //return RedirectToAction("Index", "Home");
            }


            var startDate = db.Courses.FirstOrDefault(c => c.Id == id)
                .Modules.Select(m => m.EndDate)
                .Concat(
                    db.Modules.Where(c => c.CourseId == id)
                    .Select(c => c.EndDate))
                .Max().AddDays(1);


            // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (startDate == null)
            {
                var _startDate = db.Courses.FirstOrDefault(c => c.Id == id);
                startDate = _startDate.StartDate;
            }


            var endDate = db.Courses.FirstOrDefault(c => c.Id == id);

            module.CourseId = dbCourse.Id;
            module.StartDate = startDate;
            module.EndDate = endDate.EndDate;


            return View(module);
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteCourse(int id)
        {

            var course = db.Courses.Single(i => i.Id == id);


            if (course.Modules.Count() == 0 && course.Users.Count() == 0)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            if (course.Modules.Count() > 0 && course.Users.Count() > 0)
                ViewBag.ErrorMessage = $"{course.Name} has {course.Modules.Count()} modules, and {course.Users.Count()} students enrolled.  You must delete all modules and remove all students from the course, before you can delete the actual course.";

            if (course.Modules.Count() > 0 && course.Users.Count() == 0)
                ViewBag.ErrorMessage = $"{course.Name} has {course.Modules.Count()} modules. You must delete all modules, before you can delete the actual course.";

            if (course.Users.Count() > 0 && course.Modules.Count() == 0)
                ViewBag.ErrorMessage = $"{course.Name} has {course.Users.Count()} students enrolled. You must remove all students from the course, before you can delete the actual course.";


            return View("Error");
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteModuleOld(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);

            if (module == null)
                return HttpNotFound();

            db.Modules.Remove(module);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveModule(Module module)
        {
            bool bol = false;
            //var moduleInDb = db.Modules.FirstOrDefault(m => m.Id == module.Id);

            foreach (var modules in db.Modules)
            {
                if (modules.Name == module.Name)
                {
                    bol = true;
                    break;
                }
            }



            if (bol == false)
            {
                db.Modules.Add(module);

                //var mods = db.Modules.Single(m => m.CourseId == 0);

                db.SaveChanges();

                return RedirectToAction("Course", "Home");
            }
            else
            {
                var moduleInDb = db.Modules.FirstOrDefault(m => m.Name == module.Name);
                moduleInDb.Name = module.Name;
                moduleInDb.Description = module.Description;
                moduleInDb.StartDate = module.StartDate;
                moduleInDb.EndDate = module.EndDate;
                moduleInDb.CourseId = module.CourseId;
                db.SaveChanges();
            }



            return RedirectToAction("Course", "Home");
        }

        /*[Authorize]
        public ActionResult Course()
        {
            var mods = db.Modules.Single(m => m.CourseId == 0);
            return View();
        }*/


        [Authorize]
        public ActionResult Course(int id)
        {

            /*if(id == 0)
            {
                //Course _course = new Course();
                return View();
            }*/

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
            var course = db.Courses.Find(module.CourseId);
            if (course == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "course id not found");

            if (module.StartDate < course.StartDate ||
                module.StartDate > course.EndDate ||
                module.EndDate > course.EndDate)
            {
                ModelState.AddModelError("", $"Dates must be within course time span.\n Start: {course.StartDate.ToShortDateString()} End: {course.EndDate.ToShortDateString()}");
            }
            if (module.StartDate > module.EndDate)
                ModelState.AddModelError("", $"Dates must be in sequence");


            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToLocal(returnUrl);
            }

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

        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteModuleOld(int? id, string returnUrl = "/")
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);

            if (module == null)
                return HttpNotFound();

            db.Modules.Remove(module);
            db.SaveChanges();

            return RedirectToLocal(returnUrl);
        }

        public ActionResult ListActivity(int id)
        {
            var activity = db.Activities.Where(m => m.ModuleId == id).ToList();

            return PartialView(activity);
        }
        // GET: 
        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateActivity(int? moduleId, string returnUrl = "/")
        {
            if (moduleId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // get latest end-date from module list OR the course start-date
            var startDate = db.Modules.FirstOrDefault(c => c.Id == moduleId)
                .Activity.Select(m => m.EndDate)
                .Concat(
                    db.Modules.Where(c => c.Id == moduleId)
                    .Select(c => c.StartDate))
                .Max().AddDays(1);

            var activity = new Activity
            {
                ModuleId = (int)moduleId,
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