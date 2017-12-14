using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lexicon_LMS.Controllers
{
    public class DocumentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: list student hand-ins for a specified activity, including those students who have not yet submitted a document
        [Authorize(Roles = Role.Teacher)]
        public ActionResult ListStudentDocuments(int? activityId)
        {
            var courseId = db.Activities.Find(activityId).Module.CourseId;
            var documents = db.Documents.Where(x => x.ActivityId == activityId);
            var studentDocuments = db.Users
                .Where(x => x.CourseId == courseId)
                .Select(x => new StudentDocumentViewModel
                { UserId = x.Id, FirstName = x.FirstName, LastName = x.LastName, Document = documents.FirstOrDefault(d => d.Author.Id == x.Id) })
                .OrderBy(x => x.Document == null)
                .ThenBy(x => x.LastName)
                .ThenBy(x => x.FirstName);

            var activity = db.Activities.Find(activityId);
            ViewBag.Title = $"Student assignments ( {activity.Module.Course.Name} - {activity.Module.Name} - {activity.Name} )";
            ViewBag.ReturnUrl = Url.Action("Course", "Home", new { id = activity.Module.Course.Id, ModuleId = TempData["ModuleId"] });
            return View(studentDocuments.ToList());
        }

        [Authorize]
        public ActionResult AddFileToSystem(int? courseId, int? moduleId, int? activityId, bool? isPartial)
        {
            var document = new Document
            {
                CourseId = courseId,
                ModuleId = moduleId,
                ActivityId = activityId,
            };

            TempData.Keep("ReturnUrl");
            if (isPartial ?? false)
                return PartialView(document);
            else
                return View(document);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFileToSystem(HttpPostedFileBase file, [Bind(Include = "Description,CourseId,ModuleId,ActivityId")]  Document document)
        {
            ModelState.Remove("FileName");

            document.FileName = Path.GetFileName(file.FileName);
            document.CreateTime = DateTime.Now;
            var foo = ModelState;
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                // Validate user rights:
                if (User.IsInRole(Role.Student))
                {
                    // Activity must allow studend assignments and student must be enrolled in course
                    var courseId = db.Activities.Find(document.ActivityId).Module.Course.Id;
                    if (db.Users.Find(userId).CourseId != courseId)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Student not allowed to upload in this course (student not enrolled in specified course");
                    }
                    document.StudentDocument = new StudentDocument { Graded = false };
                }
                document.Author = db.Users.Find(userId);

                db.Documents.Add(document);
                db.SaveChanges();

                // Write file to disc
                var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Docs");
                var fileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(rootPath, document.Id.ToString(), fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                file.SaveAs(fullPath);

                if (TempData["ReturnUrl"] != null)
                    return Redirect(TempData["ReturnUrl"].ToString());
                else
                    return RedirectToAction("Index", "Home");
            }
            TempData.Keep("ReturnUrl");
            return View(document);
        }

        [Authorize]
        [HttpGet]
        public FileResult GetFile(int id)
        {
            var document = db.Documents.Find(id);
            var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Docs");
            var fileName = document.FileName;
            var fullPath = Path.Combine(rootPath, document.Id.ToString(), fileName);

            return File(fullPath, "application/octet-stream", document.FileName);
        }
    }
}