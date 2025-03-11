using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quizmaster.Models
{
    public class User 
    {
        [Key]
        public int ID {get; set;}

        [Required]
        public required string EMail {get; set;}

        [Required]
        public required string Username {get; set;}

        [Required]
        public required string Password {get; set;}
    }
}