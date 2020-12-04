using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Event = new HashSet<Event>();
            Message = new HashSet<Message>();
            Subject = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }

        public virtual LoginData IdNavigation { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }

        public override string ToString()
        {
            if (SecondName!=null)
                return FirstName + " " + SecondName+ " " + Surname;
            return FirstName + " " + Surname;

        }
    }
}
