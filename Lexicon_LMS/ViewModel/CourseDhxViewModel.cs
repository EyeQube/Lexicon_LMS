using DHTMLX.Scheduler;
using Lexicon_LMS.Controllers;
using Lexicon_LMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.ViewModel
{
    public class CourseDhxViewModel
    {

        public Course Course { get; set; }
        public DHXScheduler DHX { get; set; }

        public string text { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public int CourseId { get; set; }


        public CourseDhxViewModel()
        {

        }

        public CourseDhxViewModel(Course course)
        {

            text = course.Description;
            start_date = new DateTime();
            end_date = new DateTime();
            CourseId = course.Id;

        }

    }
}