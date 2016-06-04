using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QnA.Models
{
    public class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
            Tags = new HashSet<QuestionsTags>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Votes { get; set; }

        [Required]
        public int Views { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<QuestionsTags> Tags { get; set; }
    }
}