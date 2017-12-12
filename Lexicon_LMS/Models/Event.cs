using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Event
    {
        public int id { get; set; }
            
        public string text { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }


        [Display(Name = "Parent course")]
        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }


        /* public Event(Course course)
         {
             CourseId = course.Id;
             start_date = DateTime.Now;
             end_date = DateTime.Now;
         }*/

        public Event()
        {

        }

        public Event(Event _event)
        {
            id = _event.id;

            text = _event.text;
            start_date = _event.start_date;
            end_date = _event.end_date;

            CourseId = _event.CourseId;
        }

        public Event(int i)
        {
            CourseId = i;
            start_date = DateTime.Now;
            end_date = DateTime.Now;
        }


        //[Display(Name = "Parent course")]


        /* public int CourseId { get; set; }

         [ForeignKey("id")]
         public virtual Course Course { get; set; }*/

        //[Required]



        // public int CourseId { get; set; }
        // [ForeignKey("CourseId")]
        // public virtual Course Course { get; set; }

    }   
}
