using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupStudent = new HashSet<GroupStudent>();
            GroupSubject = new HashSet<GroupSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GroupStudent> GroupStudent { get; set; }
        public virtual ICollection<GroupSubject> GroupSubject { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
