using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Http;

namespace Lexicon_LMS.Controllers.Api
{
    public class AccountApiController : ApiController
    {
        private ApplicationDbContext db;

        public AccountApiController()
        {
            db = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        [Route("AccountApi/DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(string id)
        {
            if (id == null)
                return Content(HttpStatusCode.BadRequest, "Missing id in API call");

            if (id == User.Identity.GetUserId())
                return Content(HttpStatusCode.BadRequest, "You can not remove yourself !");

            var user = db.Users.Find(id);

            if (user == null)
                return Content(HttpStatusCode.BadRequest, "Item not found in database");

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok();
        }


    }
}
