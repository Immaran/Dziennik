using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Grade = new HashSet<Grade>();
            GroupSubject = new HashSet<GroupSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Grade> Grade { get; set; }
        public virtual ICollection<GroupSubject> GroupSubject { get; set; }
        public override string ToString()
        {
            return Name + " " + TeacherId;
        }
    }
}
