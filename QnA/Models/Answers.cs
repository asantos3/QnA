using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QnA.Models
{
    public class Answers
    {

        public virtual Questions Question { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }

        [Required]
        public int Votes { get; set; }

        [Required]
        public int Views { get; set; }
    }
}