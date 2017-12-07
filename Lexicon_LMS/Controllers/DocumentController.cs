using Lexicon_LMS.Models;
using System;
using System.IO;
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