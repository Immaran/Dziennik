using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class GroupSubject
    {
        public int GroupId { get; set; }
        public int SubjectId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
