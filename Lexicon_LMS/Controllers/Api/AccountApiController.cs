using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Lexicon_LMS.Controllers.Api
{
    public class AccountApiController : ApiController
    {
        private ApplicationDbContext _context;

        public AccountApiController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize(Roles = Role.Teacher)]
        [Route("AccountApi/DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(string id)
        {
            if (id == null)
                return Content(HttpStatusCode.NotFound, "Missing id in API call");

            if (id == User.Identity.GetUserId())
                return Content(HttpStatusCode.BadRequest, "You can not remove yourself !");

            var user = _context.Users.Single(a => a.Id == id);

            if (user == null)
                return Content(HttpStatusCode.NotFound, "Item not found in database");

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }


    }
}
