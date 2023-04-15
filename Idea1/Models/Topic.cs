using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Idea1.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string Title { get; set; }

        public DateTimeOffset FirstDate { get; set; }

        public DateTimeOffset LastDate { get; set; }

        public virtual ICollection<Idea> Ideas { get; set; }

    }
}