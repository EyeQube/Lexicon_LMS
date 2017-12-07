using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Event
    {
        public int Id { get; set; }    
        public string Text { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }
}   