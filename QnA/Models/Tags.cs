using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace QnA.Models
{
    public class Tags
    {
        public Tags()
        {
            QuestionsTags = new HashSet<QuestionsTags>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        public virtual ICollection<QuestionsTags> QuestionsTags { get; set; }
    }
}