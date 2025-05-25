using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class Lobby
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Name { get; set; }

        public bool IsPrivate { get; set; }

        public string? Password { get; set; }

        public string? Description { get; set; }

        [Required]
        public int MaxPlayers { get; set; }

        [Required]
        public int CurrentPlayers { get; set; }

        [ForeignKey("Creator")]
        public int CreatorID { get; set; }
        public User? Creator { get; set; } = null!;

        [ForeignKey("Quiz")]
        public int? QuizID { get; set; }
        public Quiz? Quiz { get; set; }
    }
}