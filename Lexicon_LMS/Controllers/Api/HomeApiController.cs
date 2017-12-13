using Lexicon_LMS.Models;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Lexicon_LMS.Controllers.Api
{
    public class HomeApiController : ApiController
    {
        private ApplicationDbContext db;

        public HomeApiController()
        {
            db = new ApplicationDbContext();
        }

        [Route("HomeApi/DeleteCourse/{id}")]
        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteCourse(int? id)
        {
            if (id == null)
                return Content(HttpStatusCode.BadRequest, "Missing id in API call");

            Course course = db.Courses.Find(id);

            if (course == null)
                return Content(HttpStatusCode.BadRequest, "Item not found in database");

            var user = course.Users.FirstOrDefault();

            if (user == null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            else
            {
                db.Users.Remove(user);
                db.Courses.Remove(course);
                db.SaveChanges();
            }

            return Ok();
        }

        [Route("HomeApi/DeleteModule/{id}")]
        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteModule(int? id)
        {
            if (id == null)
                return Content(HttpStatusCode.BadRequest, "Missing id in API call");

            Module module = db.Modules.Find(id);

            if (module == null)
                return Content(HttpStatusCode.BadRequest, "Item not found in database");

            db.Documents.RemoveRange(module.Documents);
            db.Modules.Remove(module);
            db.SaveChanges();

            return Ok();
        }

        [Route("HomeApi/DeleteActivity/{id}")]
        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteActivity(int? id)
        {
            if (id == null)
                return Content(HttpStatusCode.BadRequest, "Missing id in API call");

            Activity activity = db.Activities.Find(id);

            if (activity == null)
                return Content(HttpStatusCode.BadRequest, "Item not found in database");

            db.Documents.RemoveRange(activity.Documents);
            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok();
        }

    }
}
