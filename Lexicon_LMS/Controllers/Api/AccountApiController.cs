﻿using Lexicon_LMS.Models;
using System.Linq;
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
        [Route("Api/Account/{id}")]
        public IHttpActionResult Delete(string id)
        {
            //var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(a => a.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }
    }
}
