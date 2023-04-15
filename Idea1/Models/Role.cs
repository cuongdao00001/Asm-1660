using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Role
    {
        public Role() { }
        public Role(ApplicationRole role)
        {
            RoleID = role.Id;
            RoleName = role.Name;
        }

        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }
}