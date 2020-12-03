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
            if(Value.Length == 2)
            {
                return "\t\t" + Value + "\t" + Date.ToShortDateString() + "\t" + Description;
            }
            else
            {
                return "\t\t" + Value + " \t" + Date.ToShortDateString() + "\t" + Description;
            }
            // wersja z godzinami
            //return Value + " ( " + Date.ToShortDateString() + " " + Date.ToShortTimeString() + " " + Description + " )";
        }
    }
}
