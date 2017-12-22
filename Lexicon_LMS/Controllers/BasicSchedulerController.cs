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


        public CourseID LatestCourseID = new CourseID();
                

        public BasicSchedulerController()
        {

            db = new ApplicationDbContext();

        }


        // GET: BasicScheduler
        public ActionResult Index(int? id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);


            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = course.StartDate; 


            var ViewModel = new CourseDhxViewModel()
            {
                Course = course,
                DHX = sched
            };


            var nrOfCourseIds = db.CurrentCourseID.Count();


            if(nrOfCourseIds > 0)
            {
                var CourseIdList = db.CurrentCourseID.Where(c => c.Compare == 1).ToList();

                foreach (var courseId in CourseIdList)
                {
                    db.CurrentCourseID.Remove(courseId);
                    db.SaveChanges();
                }
            }

            
            LatestCourseID.C_ID(course.Id);

            db.CurrentCourseID.Add(LatestCourseID);

            db.SaveChanges();



            return View(ViewModel);
        }

        
        public ContentResult Data()
        {

            var courseId = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
            var currentCourseID = courseId.CurrentCourse_ID;
            db.SaveChanges();       


            return (new SchedulerAjaxData(
                new SchedulerContext().Courses.Single(e => e.Id == currentCourseID).Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
                )
                );
        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var db = new ApplicationDbContext();

            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            var entities = new SchedulerContext();

            var eventz = entities.Events.Count();

            var CurrentCourseId = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
            CurrentCourseId.Event_ID = changedEvent.id;    
            db.SaveChanges();

            
                try
                {
                    switch (action.Type)
                    {
                        case DataActionTypes.Insert:

                            entities.Events.Add(changedEvent);

                            entities.SaveChanges();

                            var coreId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
                            var insertCurrentCourseID = coreId_.CurrentCourse_ID;
                                
                            var _target = entities.Events.Single(e => e.id == changedEvent.id);

                            _target.Eventz(changedEvent.text, changedEvent.start_date, changedEvent.end_date, insertCurrentCourseID);    
                        

                            break;


                        case DataActionTypes.Delete:

                            changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                            entities.Events.Remove(changedEvent);

                            break;


                        default:// "update"
                            
                            var courseId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
                            var updateCurrentCourseID = courseId_.CurrentCourse_ID;

                            var target = entities.Events.Single(e => e.id == changedEvent.id);

                            target.Eventz(changedEvent.text, changedEvent.start_date, changedEvent.end_date, updateCurrentCourseID);


                            DHXEventsHelper.Update(target, changedEvent, new List<string> { "id", "text", "start_date", "end_date" });
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







  
          