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
        public Event B_Event;      

        public int _CourseID;

        private ApplicationDbContext db;

        private SchedulerContext _dbDH;

        public BasicSchedulerController()
        {
            db = new ApplicationDbContext();

            _dbDH = new SchedulerContext();
        }


        // GET: BasicScheduler
        public ActionResult Index(int? id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);


            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = course.StartDate; //new DateTime(2016, 5, 5); //course.StartDate;  //new DateTime(2017, 11, 27);


            //var events = _dbDH.Events.FirstOrDefault(c => c.CourseId == id);

            var ViewModel = new CourseDhxViewModel()
            {
                Course = course,
                DHX = sched
            };

            _CourseID = course.Id;

            B_Event = new Event(course.Id);
            var entities = new SchedulerContext();

            entities.Events.Add(B_Event);

            return View(ViewModel);
        }


        public ContentResult Data()
        {

            return (new SchedulerAjaxData(
                new SchedulerContext().Courses.Single(e => e.Id == _CourseID).Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
                )
                );
        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
                    

            B_Event.EventHandle(changedEvent);


            var entities = new SchedulerContext();

            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        entities.Events.Add(changedEvent);
                        var target = entities.Events.Single(e => e.id == (B_Event.id - 2));
                        DHXEventsHelper.Update(target, B_Event, new List<string> { "id" });
                        //entities.Events.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        B_Event = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                        entities.Events.Remove(B_Event);
                        break;
                    default:// "update"
                        var target_ = entities.Events.Single(e => e.id == (B_Event.id - 1));
                        DHXEventsHelper.Update(target_, B_Event, new List<string> { "id" });
                        break;
                }

                entities.SaveChanges();
                action.TargetId = B_Event.id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }
    }
}