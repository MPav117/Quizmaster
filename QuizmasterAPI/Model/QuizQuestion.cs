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

        public string? OfferedAnswer1 { get; set; }
        public string? OfferedAnswer2 { get; set; }
        public string? OfferedAnswer3 { get; set; }
        public string? OfferedAnswer4 { get; set; }

        [Required]
        public required int QuestionType { get; set; }

        [Required]
        public required int PointValue { get; set; }
        
        public string? Image { get; set; }

        [ForeignKey("Quiz")]
        public int QuizID { get; set; }

        [JsonIgnore]
        public Quiz? Quiz { get; set; }

        [JsonIgnore]
        public List<LobbyLog>? Logs { get; }
    }
}