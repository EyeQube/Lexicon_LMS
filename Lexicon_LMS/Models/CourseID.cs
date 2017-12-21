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

        public int Event_ID { get; set; }   

        public CourseID(int _id)
        {
            CurrentCourse_ID = _id;
            Compare = 1;
            Event_ID = 0;
        }


        public CourseID()
        {
            CurrentCourse_ID = 0;
            Compare = 1;
            Event_ID = 0;
        }

        public void C_ID(int i)
        {
            CurrentCourse_ID = i; 

        }


    }

}