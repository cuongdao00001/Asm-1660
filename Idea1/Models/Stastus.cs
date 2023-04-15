using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Stastus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public Stastus() { }

        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}