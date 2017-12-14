using Lexicon_LMS.Models;
using System.Data.Entity;
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
                return Content(HttpStatusCode.NotFound, "Missing id in API call");

            Course course = db.Courses.Find(id);

            if (course == null)
                return Content(HttpStatusCode.NotFound, "Item not found in database");

            // Some kind of validation logic
            var validatedOk = true;
            if (!validatedOk)
                return Content(HttpStatusCode.BadRequest, "Delete request failed due to validation: <placeholder>");

            // TODO remove from filesystem
            db.Documents.RemoveRange(db.Documents.Where(x => x.Activity.Module.CourseId == course.Id));
            db.Documents.RemoveRange(db.Documents.Where(x => x.Module.CourseId == course.Id));
            db.Documents.RemoveRange(db.Documents.Where(x => x.CourseId == course.Id));
            db.SaveChanges();

            foreach (var user in course.Users.ToList())
            {
                user.CourseId = null;
                db.Entry(user).State = EntityState.Modified;
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok();
        }

        //TODO improve routing
        [Route("HomeApi/DeleteModule/{id}")]
        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteModule(int? id)
        {
            if (id == null)
                return Content(HttpStatusCode.NotFound, "Missing id in API call");

            Module module = db.Modules.Find(id);

            if (module == null)
                return Content(HttpStatusCode.NotFound, "Item not found in database");

            // Some kind of validation logic
            var validatedOk = true;
            if (!validatedOk)
                return Content(HttpStatusCode.BadRequest, "Delete request failed due to validation: <placeholder>");

            // remove documents
            // TODO remove from filesystem
            db.Documents.RemoveRange(db.Documents.Where(x => x.Activity.ModuleId == module.Id));
            db.Documents.RemoveRange(module.Documents);
            db.Modules.Remove(module);
            db.SaveChanges();

            return Ok();
        }

        //TODO improve routing
        [Route("HomeApi/DeleteActivity/{id}")]
        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteActivity(int? id)
        {
            if (id == null)
                return Content(HttpStatusCode.NotFound, "Missing id in API call");

            Activity activity = db.Activities.Find(id);

            if (activity == null)
                return Content(HttpStatusCode.NotFound, "Item not found in database");

            // Some kind of validation logic
            var validatedOk = true;
            if (!validatedOk)
                return Content(HttpStatusCode.BadRequest, "Delete request failed due to validation: <placeholder>");

            db.Documents.RemoveRange(activity.Documents);
            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok();
        }


    }
}
