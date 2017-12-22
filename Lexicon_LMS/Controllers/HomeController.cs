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


            var dbCourse = db.Courses.Single(c => c.Id == id);

            Module module = new Module();

            if (dbCourse.Modules.Count() == 0)
            {
                module.CourseId = dbCourse.Id;
                module.StartDate = dbCourse.StartDate;
                module.EndDate = dbCourse.EndDate;

                return View(module);
            }

            // get latest end-date from module list OR the course start-date
            var startDate = db.Courses.FirstOrDefault(c => c.Id == id)
                .Modules.Select(m => m.EndDate)
                .Concat(
                    db.Modules.Where(c => c.CourseId == id)
                    .Select(c => c.EndDate))
                .Max().AddDays(1);

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
        public ActionResult EditCourse(int id)
        {
            var course = db.Courses.Single(c => c.Id == id);

            return View("EditCourse", course);
        }


        [Authorize]
        public ActionResult Course(int id, int? moduleId)
        {
            TempData["ReturnUrl"] = Url.Action("Course", routeValues: new { id = id, moduleId = moduleId });

            if (moduleId != null && db.Modules.Find(moduleId) != null)
                ViewBag.ModuleId = moduleId;

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveCourse(Course course)
        {
            course.EndDate = new System.DateTime(course.EndDate.Year, course.EndDate.Month, course.EndDate.Day, 23, 59, 59);

            if (course.Id == 0)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Course", routeValues: new { id = course.Id });
            }
            else
            {
                var courseInDb = db.Courses.Single(c => c.Id == course.Id);
                courseInDb.Name = course.Name;
                courseInDb.Description = course.Description;
                courseInDb.StartDate = course.StartDate;
                courseInDb.EndDate = course.EndDate;

                db.Entry(courseInDb).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToLocal(TempData["ReturnUrl"]?.ToString());
            }
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
            {
                // ViewBag.Status = status;
                return RedirectToAction("ListCourses");
            }


            var user = db.Users.Find(User.Identity.GetUserId());
            var courseid = user?.Course?.Id;

            if (courseid != null)
                return RedirectToAction("Course", new { Id = courseid });

            return View();
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateModule(int? courseId)
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

            return View(module);
        }


        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule(Module module)
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
                module.EndDate = new System.DateTime(module.EndDate.Year, module.EndDate.Month, module.EndDate.Day, 23, 59, 59);

                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Course", routeValues: new { id = course.Id, moduleId = module.Id });
            }
            return View(module);
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult EditModule(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);
            if (module == null)
                return HttpNotFound();

            TempData.Keep("ReturnUrl");
            return PartialView(module);
        }


        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                module.EndDate = new System.DateTime(module.EndDate.Year, module.EndDate.Month, module.EndDate.Day, 23, 59, 59);
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToLocal(TempData["ReturnUrl"]?.ToString());
            }
            TempData.Keep("ReturnUrl");
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
            TempData["ModuleId"] = id;

            var activity = db.Activities.Where(m => m.ModuleId == id).ToList();

            var module = db.Modules.Find(id);

            TempData["ReturnUrl"] = Url.Action("Course", routeValues: new { id = module.CourseId, moduleId = module.Id });

            return PartialView(module);
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateActivity(int? id)
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

            TempData.Keep("ReturnUrl");
            return View(activity);
        }


        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(Activity activity)
        {
            var module = db.Modules.Find(activity.ModuleId);
            if (module == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "module id not found");

            if (activity.StartDate < module.StartDate ||
                activity.StartDate > module.EndDate ||
                activity.EndDate > module.EndDate)
            {
                ModelState.AddModelError("", $"Dates must be within module time span.\n Start: {module.StartDate.ToShortDateString()} End: {module.EndDate.ToShortDateString()}");
            }
            if (activity.StartDate > activity.EndDate)
                ModelState.AddModelError("", $"Dates must be in sequence");

            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToLocal(TempData["ReturnUrl"]?.ToString());
            }
            activity.ActivityTypes = db.ActivityTypes.ToList();

            TempData.Keep("ReturnUrl");
            return View(activity);
        }


        [Authorize(Roles = Role.Teacher)]
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity activity = db.Activities.Find(id);
            if (activity == null)
                return HttpNotFound();

            activity.ActivityTypes = db.ActivityTypes.ToList();

            TempData.Keep("ReturnUrl");
            return PartialView(activity);
        }


        [Authorize(Roles = Role.Teacher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity([Bind(Include = "Id,Name,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToLocal(TempData["ReturnUrl"]?.ToString());
            }

            TempData.Keep("ReturnUrl");
            activity.ActivityTypes = db.ActivityTypes.ToList();
            return View(activity);
        }

        //TODO Refactor this method? Copy exists in other controller...
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


    }
}