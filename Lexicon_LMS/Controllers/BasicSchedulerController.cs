using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using Lexicon_LMS.Models;
using Lexicon_LMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lexicon_LMS.Controllers
{
    public class BasicSchedulerController : Controller
    {

        private ApplicationDbContext db;

        //private SchedulerContext _dbDH;

        public BasicSchedulerController()
        {
            db = new ApplicationDbContext();

          //  _dbDH = new SchedulerContext();

        }


        // GET: BasicScheduler
        public ActionResult Index(int? id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);

            //var events = _dbDH.Events.FirstOrDefault(c => c.CourseId == id);


            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = course.StartDate; //new DateTime(2016, 5, 5); //course.StartDate;  //new DateTime(2017, 11, 27);
          

            /*public ActionResult Index()
            {
                var sched = new DHXScheduler(this);
                sched.Skin = DHXScheduler.Skins.Terrace;
                sched.LoadData = true;
                sched.EnableDataprocessor = true;
                sched.InitialDate = new DateTime(2016, 5, 5);
                return View(sched);
            }*/

            //var events = _dbDH.Events.FirstOrDefault(c => c.CourseId == id);

            var ViewModel = new CourseDhxViewModel()
            {
                Course = course,
                DHX = sched
            };

            // ViewBag.Id = course.Id;

            return View(ViewModel);
        }

        
        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new SchedulerContext().Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date})
                )
                );
        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            var entities = new SchedulerContext();
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        entities.Events.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                        entities.Events.Remove(changedEvent);
                        break;
                    default:// "update"
                        var target = entities.Events.Single(e => e.id == changedEvent.id);
                        DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });
                        break;
                }
                entities.SaveChanges();
                action.TargetId = changedEvent.id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }
    }
}