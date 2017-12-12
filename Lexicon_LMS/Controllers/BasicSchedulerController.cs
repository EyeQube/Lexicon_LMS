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
        public Event B_Event { get; set; }  
        public int CourseId;
        public Course _Course { get; set; }
        public CourseDhxViewModel ViewModel { get; set; }   

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

            //var events = _dbDH.Events.FirstOrDefault(c => c.CourseId == id);


            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = course.StartDate; //new DateTime(2016, 5, 5); //course.StartDate;  //new DateTime(2017, 11, 27);
          

            ViewModel = new CourseDhxViewModel(course)
            {
                Course = course,
                DHX = sched
            };


            CourseId = course.Id;

            //_Course = course;

            B_Event = new Event();

            B_Event.CourseId = _Course.Id;

            var entities = new SchedulerContext();

            entities.Events.Add(B_Event);

            var gyt = entities.Events.FirstOrDefault(c => c.CourseId == id);

            B_Event = gyt;
            // entities.Events.Add(B_Event);

            //entities.SaveChanges();

            return View(ViewModel);
        }

        
        public ContentResult Data()
        {
            //var entities = new SchedulerContext();

            return (new SchedulerAjaxData(
             new SchedulerContext().Events
             .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
             )
             );

        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            B_Event = new Event(CourseId);

            var action = new DataAction(actionValues);

            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);

            //B_Event.id = changedEvent.id;

              changedEvent.CourseId = B_Event.CourseId;

              B_Event.start_date = changedEvent.start_date;
              B_Event.end_date = changedEvent.end_date;
              B_Event.text = changedEvent.text;

            var entities = new SchedulerContext();

            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        //entities.Events.Add(changedEvent);
                        entities.Events.Add(B_Event);
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

               // var Ev = entities.Events.FirstOrDefault(i => i.id == id);
               // Ev.CourseId = _Course.Id;


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