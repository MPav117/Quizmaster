using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class LobbySession
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Lobby")]
        public int LobbyID { get; set; }
        public Lobby Lobby { get; set; } = null!;

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public string? ConnectionId { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public bool Ready { get; set; }

        [Required]
        public required bool Incorrect { get; set; }

        public string? Answer { get; set; }
    }
}