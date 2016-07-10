namespace QnA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public class QuestionsVotes : DbContext
    {
        public virtual Questions Question { get; set; }
        [Key]
        [ForeignKey("Question")]
        [Column(Order = 1)]
        public int QuestionID { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Key]
        [ForeignKey("User")]
        [Column(Order = 2)]
        public string UserID { get; set; }

        [Required]
        public Boolean VotedPositive { get; set; }

        [Required]
        public Boolean VotedNegative { get; set; }
    }
}