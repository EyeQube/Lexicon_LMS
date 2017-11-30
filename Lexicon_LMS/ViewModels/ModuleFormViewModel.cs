using Lexicon_LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Lexicon_LMS.ViewModels
{
    public class ModuleFormViewModel
    {
        public int Id { get; set; } 
        public ICollection<Models.Module> Modules{ get; set; }
        public Course Course { get; set; }
    }
}