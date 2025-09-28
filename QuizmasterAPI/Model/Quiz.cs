using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class Quiz
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int QuestionCount { get; set; }

        public string? Description { get; set; }
        public List<QuizQuestion>? Questions { get; }

        [ForeignKey("User")]
        public int CreatorID { get; set; }
        public User? Creator { get; set; }
    }
}