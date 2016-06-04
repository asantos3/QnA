using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QnA.Models
{
    public class Answers
    {
        [Key]
        public int ID { get; set; }

        public virtual Questions Question { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Votes { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
    }
}