using Lexicon_LMS.Models;
using System.Net;
using System.Web.Http;

namespace Lexicon_LMS.Controllers.Api
{
    public class HomeController : ApiController
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult DeleteModule(int? id)
        {
            if (id == null)
                return StatusCode(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);

            if (module == null)
                return StatusCode(HttpStatusCode.NotFound);

            db.Modules.Remove(module);
            db.SaveChanges();

            return Ok();
        }


    }
}
