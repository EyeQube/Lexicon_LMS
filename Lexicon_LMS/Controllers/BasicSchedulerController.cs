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

        public int _CourseID { get; set; }

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
          

            //var events = _dbDH.Events.FirstOrDefault(c => c.CourseId == id);

            var ViewModel = new CourseDhxViewModel()
            {
                Course = course,
                DHX = sched
            };

            _CourseID = course.Id;

            //Event _event = new Event(course.Id);

           B_Event = new Event(course.Id);

           var entities = new SchedulerContext();

            //entities.Events.Add(_event);
           entities.Events.Add(B_Event);

           entities.SaveChanges();

            return View(ViewModel);
        }

        
        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new SchedulerContext().Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
                )
                );
        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);
            Event changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            var entities = new SchedulerContext();

            //changedEvent.CourseId = _CourseID;

            //changedEvent.CourseId = B_Event.CourseId;

            //var efrf = entities.Events.Single(e => e.)
            //_event = entities.Events.FirstOrDefault(n => n.id == id);
            //var target = entities.Events.Single(e => e.id == B_Event.id);
            //DHXEventsHelper.Update(target, changedEvent, new List<string> { "CourseId" });

            //var _event = entities.Events.FirstOrDefault(ev => ev.id == id);   
            // entities.Events.Remove(_event);
            // entities.SaveChanges();

            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                         var target = entities.Events.Single(e => e.id == (changedEvent.id - 1));
                         DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });
                         //changedEvent = entities.Events.FirstOrDefault(ev => ev.id == (action.SourceId));
                        // entities.Events.Remove(changedEvent);
                        //entities.Events.Add(changedEvent);
                        break; 
                    // entities.Events.Add(changedEvent);
                    // break;
                    case DataActionTypes.Delete:
                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId - 1);
                        entities.Events.Remove(changedEvent);
                        //changedEvent = entities.Events.FirstOrDefault(ev => ev.id == (action.SourceId));
                        //entities.Events.Remove(changedEvent);
                        break;
                    default:// "update"
                        target = entities.Events.Single(e => e.id == (changedEvent.id - 1));
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