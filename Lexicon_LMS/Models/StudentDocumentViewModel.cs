namespace Lexicon_LMS.Models
{
    public class StudentDocumentViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Document Document { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}