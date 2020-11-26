using System;
using System.Collections.Generic;

namespace SBD.Models
{
    public partial class LoginData
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }

        public override string ToString()
        {
            return Login + " " + Password;
        }
    }
}
