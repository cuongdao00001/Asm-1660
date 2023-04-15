using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }

        public string Title { get; set; }
     
        public string Brief { get; set; }
       
        public string FileName { get; set; }
        public string FileData { get; set; }
        public string Content { get; set; }
        
        public bool IsAnonymous { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int Views { get; set; }
       public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }   
        public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual List<Comment> Comments { get; set; }


        public int IdeaCount { get; set; }


    }
}