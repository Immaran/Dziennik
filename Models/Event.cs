using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public override string ToString()
        {
            if(Description != null)
            {
                return Name + "\n" + Description + "\n" + Date.ToShortDateString() + " " + Date.ToShortTimeString();
            }
            else return Name + "\n" + Date.ToShortDateString() + " " + Date.ToShortTimeString();
        }
    }
}
