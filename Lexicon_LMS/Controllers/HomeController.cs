using Lexicon_LMS.Models;
using System.Linq;
using System.Web.Mvc;

namespace Lexicon_LMS.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Course(int Id)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == Id);

            var u = course.Users.ToList();
            if (course != null)
                return View(course);
            else
                return HttpNotFound();
        }

        public ActionResult Index()
        {
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