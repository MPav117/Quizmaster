using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Quizmaster.Datatypes;
using Quizmaster.Models;

namespace Quizmaster.Interfaces
{
    public interface IAuthService
    {
        public Task<ReturnValue<JwtSecurityToken>> Login(LoginInfo newLoginInfo);
        public Task<ReturnValue<string>> Register(RegisterInfo newUserInfo);
        public Task<ReturnValue<User>> GetClaimedUser(StringValues authHeader);
        public JwtSecurityToken GetJwtSecurityToken(User user, string key);
        public Task<ReturnValue<JwtSecurityToken>> RefreshJwtSecurityToken();
    }
}