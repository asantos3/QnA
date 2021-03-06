using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QnA.Models
{
    public class Answers
    {
        public Answers()
        {
            AnswersVotes = new HashSet<AnswersVotes>();
        }

        [Key]
        public int ID { get; set; }

        public virtual Questions Question { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        [Required]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Votes { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }

        // So it's possible to detect if the answer was upvoted or downvoted
        public virtual ICollection<AnswersVotes> AnswersVotes { get; set; }
    }
}