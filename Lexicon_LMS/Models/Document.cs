using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexicon_LMS.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Display(Name = "Document name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Created")]
        public DateTime CreateTime { get; set; }

        //// Activity/hand-in properties - not part of user story D1
        //public DateTime ResponseTime { get; set; }
        //public DateTime CompleteTime { get; set; }
        //public Boolean Completed { get; set; }

        [Required]
        [Display(Name = "File name")]
        [StringLength(255)]
        public string FileName { get; set; }

        public string RelativePath => Id + "\\" + FileName;

        // Keys
        //public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }

        public int? StudentDocumentId { get; set; }

        // Navigation
        //[ForeignKey(" ApplicationUserId")]
        public virtual ApplicationUser Author { get; set; }    // Documents for students/teachers 

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }

        [ForeignKey("StudentDocumentId")]
        public virtual StudentDocument StudentDocument { get; set; }

    }
}