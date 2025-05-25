using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quizmaster.Datatypes
{
    public class RegisterInfo
    {
        [JsonConstructor]
        public RegisterInfo(string username, string email, string password)
        {
            Username = username;
            EMail = email;
            Password = password;
        }

        public string Username {get; set;}
        public string EMail {get; set;}
        public string Password {get; set;}
    }
}