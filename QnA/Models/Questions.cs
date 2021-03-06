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
            QuestionsVotes = new HashSet<QuestionsVotes>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Votes { get; set; }

        [Required]
        public int Views { get; set; }

        public int? CorrectAnswerID { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<QuestionsTags> Tags { get; set; }
        // So it's possible to detect if the question was upvoted or downvoted
        public virtual ICollection<QuestionsVotes> QuestionsVotes { get; set; }
    }
}