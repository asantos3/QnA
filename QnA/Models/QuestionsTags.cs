namespace QnA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public class QuestionsTags : DbContext
    {
        public virtual Questions Question { get; set; }
        [Key]
        [ForeignKey("Question")]
        [Column(Order = 1)]
        public int QuestionID { get; set; }

        public virtual Tags Tag { get; set; }
        [Key]
        [ForeignKey("Tag")]
        [Column(Order = 2)]
        public int TagID { get; set; }
    }
}