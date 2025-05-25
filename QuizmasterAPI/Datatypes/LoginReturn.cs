using System.IdentityModel.Tokens.Jwt;
using Quizmaster.Models;

namespace Quizmaster.Datatypes
{
    public struct LoginReturn {
        public User user;
        public JwtSecurityToken token;
    }
}