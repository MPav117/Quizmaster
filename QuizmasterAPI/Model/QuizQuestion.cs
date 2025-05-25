using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class QuizQuestion
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Question { get; set; }

        [Required]
        public required string Answer { get; set; }

        [Required]
        public required int QuestionType { get; set; }

        public string? Image { get; set; }

        [ForeignKey("Quiz")]
        public int QuizID { get; set; }
        public Quiz? Quiz { get; set; }
    }
}