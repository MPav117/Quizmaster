using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class LobbyLog
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required bool Answered { get; set; }

        [ForeignKey("Lobby")]
        public int LobbyID { get; set; }
        public Lobby Lobby { get; set; } = null!;

        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public QuizQuestion Question { get; set; } = null!;

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User? User { get; set; }

    }
}
