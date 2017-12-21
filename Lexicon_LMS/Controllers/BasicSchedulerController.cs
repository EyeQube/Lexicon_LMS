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
       // public Event B_Event { get; set; }

        public int _CourseID;

        private ApplicationDbContext db;

        private SchedulerContext _dbDH;

        public bool boll = false;

        public CourseID C_Id = new CourseID();


        public BasicSchedulerController()
        {
            //B_Event = new Event(_CourseID, DateTime.Now, DateTime.Now);

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


            var count = db.CurrentCourseID.Count();


            if(count > 0)
            {
                var n = db.CurrentCourseID.Where(c => c.Compare == 1).ToList();

                foreach (var vp in n)
                {
                    db.CurrentCourseID.Remove(vp);
                    db.SaveChanges();
                }
            }

            
            C_Id.C_ID(_CourseID);

            db.CurrentCourseID.Add(C_Id);

            db.SaveChanges();


           // B_Event._Event(course.Id);
           // var entities = new SchedulerContext();

           // entities.Events.Add(B_Event);
           // entities.SaveChanges();


            return View(ViewModel);
        }

        
        public ContentResult Data()
        {

            /* return (new SchedulerAjaxData(
                 new SchedulerContext().Events
                 .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
                 )
                 );     */


            var der = db.CurrentCourseID.FirstOrDefault(s => s.Compare == 1);
            var ter = der.CurrentCourse_ID;
            db.SaveChanges();

            return (new SchedulerAjaxData(
                new SchedulerContext().Courses.Single(e => e.Id == ter).Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date, e.CourseId})
                )
                );
        }


        [Authorize(Roles = Role.Teacher)]
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var db = new ApplicationDbContext();
            Event B_Event = new Event();

            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            var entities = new SchedulerContext();

            var eventz = entities.Events.Count(); 

             var event_id = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
             event_id.Event_ID = changedEvent.id;
             db.SaveChanges();

            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                      
                        entities.Events.Add(changedEvent);

                        entities.SaveChanges();

                        var coreId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
                        var j = coreId_.CurrentCourse_ID;
                            
                        var _target = entities.Events.Single(e => e.id == changedEvent.id);
                        _target.CourseId = j;
                        _target.text = changedEvent.text;
                        _target.end_date = changedEvent.end_date;
                        _target.start_date = changedEvent.start_date;
                        
                        break;


                    case DataActionTypes.Delete:

                        changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                        entities.Events.Remove(changedEvent);
                        
                        break;


                    default:// "update"

                        var courseId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
                        var f = courseId_.CurrentCourse_ID;

                        var target = entities.Events.Single(e => e.id == changedEvent.id);
                        target.CourseId = f;
                        target.text = changedEvent.text;
                        target.end_date = changedEvent.end_date;
                        target.start_date = changedEvent.start_date;
                        DHXEventsHelper.Update(target, changedEvent, new List<string> { "id", "text", "start_date", "end_date" });
                        break;
                }


              /*  var event_z = entities.Events.Count();

                if (event_z > eventz)
                {
                    var courseId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);
                    var f = courseId_.CurrentCourse_ID;
                    //var k = courseId_.Event_ID;

                    var targets = entities.Events.Single(e => e.id == changedEvent.id);
                    targets.CourseId = f;
                } */


                //db.SaveChanges();
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




    /*
         [Authorize(Roles = Role.Teacher)]
         public ContentResult Save(int? id, FormCollection actionValues)
         {


         var entities = new SchedulerContext();
             var db = new ApplicationDbContext();

             var courseId_ = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);

             var f = courseId_.CurrentCourse_ID;

               B_Event._Event(f);
               entities.Events.Add(B_Event);

               db.SaveChanges();
               entities.SaveChanges(); 


             var action = new DataAction(actionValues);
                 var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
                 var w = changedEvent.id;
                 //var n = changedEvent.id - 1;  */


    /*     try
         {
             switch (action.Type)
             {
                 case DataActionTypes.Insert:
                     entities.Events.Add(changedEvent);

                     entities.SaveChanges();

                     var _target = entities.Events.Single(e => e.id == (changedEvent.id - 1));
                     _target.text = changedEvent.text;
                     _target.start_date = changedEvent.start_date;
                     _target.end_date = changedEvent.end_date;

                     entities.SaveChanges();

                     var c__id = db.CurrentCourseID.FirstOrDefault(c => c.Compare == 1);

                     CourseID cv = new CourseID();
                     cv.Event_ID = (changedEvent.id - 1); //****************  cv.Event.ID(changedEvent.id - 1) ***************************
                     cv.CurrentCourse_ID = c__id.CurrentCourse_ID;
                     db.CurrentCourseID.Add(cv);

                     db.SaveChanges(); //************************

     */
    //target.CourseId = n;  //***********  

    //  entities.SaveChanges();

    //changedEvent = entities.Events.FirstOrDefault(ev => ev.id == changedEvent.id);
    //entities.Events.Remove(changedEvent);


    /*      var nor = entities.Events.Where(c => c.CourseId == null).ToList();

          foreach (var d in nor)
          {
              entities.Events.Remove(d);
              entities.SaveChanges();
          }   
           */


    //DHXEventsHelper.Update(target, changedEvent); //, new List<string> { "id" });
    // entities.SaveChanges(); //*****

    // boll = true;

    /*      break;

      case DataActionTypes.Delete:
          changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
          entities.Events.Remove(changedEvent);
          entities.SaveChanges();
          break;

      default:// "update"

           var noll = entities.Events.Where(c => c.text == null).ToList();

          foreach (var d in noll)
          {
              entities.Events.Remove(d);
              entities.SaveChanges();
          } 


          var CID = db.CurrentCourseID.Where(c => c.Compare == 1).ToList();

          var gets = db.CurrentCourseID.Single(t => t.Event_ID != 0); ///////********************

          var g = db.CurrentCourseID.Count();


              var target = entities.Events.Single(e => e.id == changedEvent.id);//changedEvent.id);
              target.text = changedEvent.text;


              //DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });

              entities.SaveChanges();



          //   else
          //  {
              //var target = entities.Events.Single(e => e.id == changedEvent.id);//changedEvent.id);
             // target.text = changedEvent.text;
             // entities.SaveChanges();

         // }


          for (var e = g; e > 0; e--)
          {
              db.CurrentCourseID.Remove(CID[e]);
              db.SaveChanges();
          }

          db.SaveChanges();




            var nollz = entities.Events.Where(c => c.text == null).ToList();   //(c => c.text == null).ToList();


             foreach (var d in nollz)
            {
              entities.Events.Remove(d);
              entities.SaveChanges();
            } 


        //  }
         /* else
          {
              var target = entities.Events.Single(e => e.id == changedEvent.id - 1);
              target.text = changedEvent.text;



              var nollz = entities.Events.Where(c => c.CourseId == null).ToList();   //(c => c.text == null).ToList();

              foreach (var d in nollz)
              {
                  entities.Events.Remove(d);
                  entities.SaveChanges();
              }

          } */

    /////

    /*var nollz = entities.Events.Where(c => c.CourseId == null).ToList();   //(c => c.text == null).ToList();

    foreach (var d in nollz)
    {
        entities.Events.Remove(d);
        entities.SaveChanges();
    }*/



    //_target_.CourseId = n;

    //var targett = entities.Events.Single(e => e.id == changedEvent.id - 1);

    //DHXEventsHelper.Update(targett , changedEvent, new List<string> { "id" });

    //entities.SaveChanges();

    //changedEvent = entities.Events.FirstOrDefault(ev => ev.id == changedEvent.id);
    //entities.Events.Remove(changedEvent);

    //entities.SaveChanges(); //*****

    //***************************************** Loop

    //entities.SaveChanges();  */

    /*           break;
       }

       //boll = false;

       /*_CourseID = course.Id;
       B_Event._Event(course.Id);
       var entities = new SchedulerContext();
       entities.Events.Add(B_Event);*/

    /*    entities.SaveChanges(); 

        var oll = entities.Events.Where(c => c.text == null).ToList();

        foreach (var d in oll)
        {
            entities.Events.Remove(d);
            entities.SaveChanges();
        }


        action.TargetId = changedEvent.id;

    }
    catch (Exception a)
    {
        action.Type = DataActionTypes.Error;
    }


    return (new AjaxSaveResponse(action));
} */

  
  }          