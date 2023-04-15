using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class User
    {
        public User(ApplicationUser user)
        {
            ApplicationUser = user;
            Name = user.UserName;
            Ideas = new List<Idea>();
        }
        public string UserId
        {
            get { return this.ApplicationUser.Id; }
            set { this.ApplicationUser.Id = value; }
        }
        public string Name { get; set; }
        public virtual ICollection<Idea> Ideas { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

}
