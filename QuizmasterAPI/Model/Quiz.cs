using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Model
{
    public class Quiz 
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int QuestionCount { get; set; }

        [Required]
        public List<Question>? Questions { get; } 

        public string? Description { get; set; }
    }
}