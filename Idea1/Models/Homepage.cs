using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Homepage
    {
        public List<Department> Departments { get; set; }
     
        public List<Topic> Topics { get; set; }      
        public List<Idea> Ideas { get; set; }
    }
}