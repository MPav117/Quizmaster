using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Quizmaster.Models;

namespace Quizmaster.Datatypes
{
    public class LoginResponse
    {
        [JsonConstructor]
        public LoginResponse(User user, string jwtToken)
        {
            User = user;
            JwtToken = jwtToken;
        }

        public User User {get; set;}
        public string JwtToken {get; set;}
    }
}