﻿using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;

namespace Lexicon_LMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }


        [Authorize] 
        public ActionResult Module(int id)
        {

            // get latest end-date from module list OR the course start-date

            var dbCourse = db.Courses.Single(c => c.Id == id);

            Module module = new Module();

            if (dbCourse.Modules.Count() == 0)
            {
                module.CourseId = dbCourse.Id;
                module.StartDate = dbCourse.StartDate;
                module.EndDate = dbCourse.EndDate;

                /*var _startDate = db.Courses.FirstOrDefault(c => c.Id == id);
                startDate = _startDate.StartDate; */

                return View(module);

                //return RedirectToAction("Index", "Home");
            }

           
            var startDate = db.Courses.FirstOrDefault(c => c.Id == id)
                .Modules.Select(m => m.EndDate)
                .Concat(
                    db.Modules.Where(c => c.CourseId == id)
                    .Select(c => c.EndDate))
                .Max().AddDays(1);


            // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (startDate == null)
            {
                var _startDate = db.Courses.FirstOrDefault(c => c.Id == id);
                startDate = _startDate.StartDate;
            }


            var endDate = db.Courses.FirstOrDefault(c => c.Id == id);

            module.CourseId = dbCourse.Id;
            module.StartDate = startDate;
            module.EndDate = endDate.EndDate;     


            return View(module);
        }



        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteModuleOld(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Module module = db.Modules.Find(id);

            if (module == null)
                return HttpNotFound();

            db.Modules.Remove(module);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveModule(Module module)  
        {
            bool bol = false;
            //var moduleInDb = db.Modules.FirstOrDefault(m => m.Id == module.Id);

            foreach (var modules in db.Modules)
            {
                if(modules.Name == module.Name)
                {
                    bol = true;
                    break;
                }
            }

            if (bol == false)
            {
                db.Modules.Add(module);
            }
             else
            {
                var moduleInDb = db.Modules.FirstOrDefault(m => m.Name == module.Name);
                moduleInDb.Name = module.Name;
                moduleInDb.Description = module.Description;
                moduleInDb.StartDate = module.StartDate;
                moduleInDb.EndDate = module.EndDate;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }  
           

        [Authorize]
        public ActionResult Course(int id)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null)
                return HttpNotFound();

            if (User.IsInRole(Role.Student))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user.CourseId != course.Id)
                {
                    ViewBag.ErrorMessage = $"You have no access to course ({course.Name})";
                    return View("Error");
                }
            }
            return View(course);
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Register()
        {
            var Course = new Course();

            return View("RegisterCourse", Course);
        }

        //
        // POST: /Home/SaveCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult SaveCourse(Course course)
        {

            if (course.Id == 0)
            {
                db.Courses.Add(course);
            }
            else
            {
                var courseInDb = db.Courses.Single(c => c.Id == course.Id);
                courseInDb.Name = course.Name;
                courseInDb.Description = course.Description;
                courseInDb.StartDate = course.StartDate;
                courseInDb.EndDate = course.EndDate;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListCourses()
        {
            var Courses = db.Courses.ToList();

            return View(Courses);
        }

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole(Role.Teacher))
                return RedirectToAction("ListCourses");

            var user = db.Users.Find(User.Identity.GetUserId());
            var courseid = user?.Course?.Id;

            if (courseid != null)
                return RedirectToAction("Course", new { Id = courseid });

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}