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


    }
}