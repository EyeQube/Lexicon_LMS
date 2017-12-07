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
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ending Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        public virtual ActivityType ActivityType { get; set; }

        [NotMapped]
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}