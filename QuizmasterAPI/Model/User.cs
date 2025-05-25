using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string EMail { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required int Experience { get; set; }

        [Required]
        public required int Level { get; set; }

        public string? ProfilePicture { get; set; }

        [ForeignKey("InLobby")]
        public int InLobbyID { get; set; }
        public Lobby? InLobby { get; set; }

        [JsonIgnore]
        public List<Quiz>? Quizzes { get; set; }
    }
}