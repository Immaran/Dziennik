using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public override string ToString()
        {
            return Content + "\nData: " + Date.ToString();
        }
    }
}
