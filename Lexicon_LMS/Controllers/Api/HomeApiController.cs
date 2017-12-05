using Lexicon_LMS.Models;
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

            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok();
        }


    }
}
