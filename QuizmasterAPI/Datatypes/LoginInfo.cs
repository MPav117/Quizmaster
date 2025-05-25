using System.Text.Json.Serialization;

namespace Quizmaster.Datatypes
{
    public class LoginInfo
    {
        [JsonConstructor]
        public LoginInfo(string email, string password)
        {
            EMail = email;
            Password = password;
        }

        public string EMail {get; set;}
        public string Password {get; set;}
    }
}