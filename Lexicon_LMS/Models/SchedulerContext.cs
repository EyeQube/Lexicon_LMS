using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public partial class SchedulerContext : ApplicationDbContext
    {
        public SchedulerContext() : base("name=SchedulerContext") { }
        public virtual DbSet<Event> Events { get; set; }
        
    }
}