using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Grade
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public override string ToString()
        {
            return Value + " " + Description + " " + Student.FirstName + " " + Student.SecondName + " " + Date.ToString();
        }
    }
}
