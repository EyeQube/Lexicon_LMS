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
            B_Event = new Event(_CourseID, DateTime.Now, DateTime.Now );

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

            B_Event._Event(course.Id);
            var entities = new SchedulerContext();

            entities.Events.Add(B_Event);
            entities.SaveChanges();

            return View(ViewModel);
        }


        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new SchedulerContext().Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date })
                )
                ); 

            /*return (new SchedulerAjaxData(
                new SchedulerContext().Courses.Single(e => e.Id == _CourseID).Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date})
                )
                );*/ 
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

                        entities.SaveChanges();

                        var target = entities.Events.Single(e => e.id == (changedEvent.id - 1));
                        target.text = changedEvent.text;
                        target.start_date = changedEvent.start_date;
                        target.end_date = changedEvent.end_date;

                        entities.SaveChanges();

                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == changedEvent.id);
                        entities.Events.Remove(changedEvent);

                        //DHXEventsHelper.Update(target, changedEvent); //, new List<string> { "id" });
                        entities.SaveChanges(); //*****

                        break;
                    case DataActionTypes.Delete:
                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                        entities.Events.Remove(changedEvent);
                        entities.SaveChanges();
                        break;
                    default:// "update"

                        entities.Events.Add(changedEvent);

                        entities.SaveChanges();

                        var target_ = entities.Events.Single(e => e.id == (changedEvent.id - 1));
                        target_.text = changedEvent.text;
                        target_.start_date = changedEvent.start_date;
                        target_.end_date = changedEvent.end_date;

                        entities.SaveChanges();

                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == changedEvent.id);
                        entities.Events.Remove(changedEvent);

                        //DHXEventsHelper.Update(target, changedEvent); //, new List<string> { "id" });
                        entities.SaveChanges(); ///**
 

                        var _target_ = entities.Events.Single(e => e.id == changedEvent.id - 3);
                        DHXEventsHelper.Update(_target_, changedEvent, new List<string> { "id", "CourseId" });

                        entities.SaveChanges(); 

                        break;
                }


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