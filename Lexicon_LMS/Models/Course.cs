using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Models
{
    public class Course
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1200)]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ending Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }


        public virtual ICollection<Module> Modules { get; set; }
    }

}