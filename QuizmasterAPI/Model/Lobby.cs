using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class Lobby 
    {
        [Key]
        public int ID {get; set;}

        [Required]
        public int Name {get; set;}

        [Required]
        public int MaxPlayers {get; set;}

        [Required]
        public int CurrentPlayers {get; set;}
        
        [ForeignKey("LobbyQuiz")]
        public int QuizID { get; set; }
        public virtual Quiz LobbyQuiz { get; set; }
    }
}