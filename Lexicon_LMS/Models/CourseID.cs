using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class CourseID
    {
        public int Id { get; set; }

        public int Compare { get; set; }

        public int CurrentCourse_ID { get; set; }   


         public CourseID()
        {
            CurrentCourse_ID = 0;
            Compare = 1;
        }

         public void InputID(int i)
        {
            CurrentCourse_ID = i; 

        } 
    }
}