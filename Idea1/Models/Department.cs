using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
   
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Idea> Ideas { get; set; }
    }
}