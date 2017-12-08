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
        public int Id { get; set; }

        public string text { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        [Required]
        public int CourseId { get; set; }
        
        [Required]
        public virtual Course Course { get; set; }


    }   
}
