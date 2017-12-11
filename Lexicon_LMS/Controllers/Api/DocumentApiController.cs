using Lexicon_LMS.Models;
using System.Net;
using System.Web.Http;

namespace Lexicon_LMS.Controllers.Api
{

    public class DocumentApiController : ApiController
    {
        private ApplicationDbContext db;

        public DocumentApiController()
        {
            db = new ApplicationDbContext();
        }

        //POST: allow teachers to toggle graded status on student submitted documents
        [Route("DocumentApi/SetGrade/{studentDocumentId}")]
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public IHttpActionResult SetGrade(int? studentDocumentId)
        {
            if (studentDocumentId == null)
                return Content(HttpStatusCode.BadRequest, "Missing id in API call");

            var studentDocument = db.StudentDocuments.Find(studentDocumentId);

            if (studentDocument == null)
                return Content(HttpStatusCode.BadRequest, $"Database entity not found for id:{studentDocumentId}");

            studentDocument.Graded = !studentDocument.Graded;
            db.SaveChanges();
            
            return Ok( new { IsGraded = studentDocument.Graded});
        }
    }

   
}