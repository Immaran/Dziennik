using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Student
    {
        public Student()
        {
            Grade = new HashSet<Grade>();
            GroupStudent = new HashSet<GroupStudent>();
            Message = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }

        public virtual LoginData IdNavigation { get; set; }
        public virtual ICollection<Grade> Grade { get; set; }
        public virtual ICollection<GroupStudent> GroupStudent { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public override string ToString()
        {
            return FirstName + " " + Surname;
        }
    }
}
