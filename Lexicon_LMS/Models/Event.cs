using Lexicon_LMS.ViewModel;
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
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }


        public Event()
        {

        }


         public Event(int id)
        {
          //  text = course.Description;
          //  start_date = course.StartDate;
          //  end_date = course.EndDate;
            CourseId = id;

        } 


    }   
}
