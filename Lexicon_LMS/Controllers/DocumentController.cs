using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lexicon_LMS.Controllers
{
    public class DocumentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Document
        public ActionResult AddFileToSystem(int? courseId, int? moduleId, int? activityId)
        {
            var document = new Document
            {
                CourseId = courseId,
                ModuleId = moduleId,
                ActivityId = activityId,
            };

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFileToSystem(HttpPostedFileBase file, Document document)
        {
            document.FileName = Path.GetFileName(file.FileName);
            document.CreateTime = DateTime.Now;

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
                }
                document.Author = db.Users.Find(userId);

                db.Documents.Add(document);
                db.SaveChanges();
                var rootPath = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(rootPath, "App_Docs", document.Id.ToString(), fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                file.SaveAs(fullPath);
                return RedirectToAction("Index", "Home");
            }
            return View(document);
        }

        [HttpGet]
        public FileResult GetFile(int id)
        {
            var document = db.Documents.Find(id);
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(rootPath, "App_Docs", id.ToString());
            var fileName = document.FileName;
            var fullPath = Path.Combine(path, fileName);

            return File(fullPath, "application/octet-stream", document.FileName);
        }
    }
}