using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexicon_LMS.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1200)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime EndDate { get; set; }

        [Required]
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }

        [Required]
        [Display(Name = "Activity type")]
        public int ActivityTypeId { get; set; }

        [Display(Name = "Activity")]
        public virtual ActivityType ActivityType { get; set; }

        [NotMapped]
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}