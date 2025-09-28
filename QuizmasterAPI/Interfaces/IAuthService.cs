using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Quizmaster.Datatypes;
using Quizmaster.Models;

namespace Quizmaster.Interfaces
{
    public interface IAuthService
    {
        public Task<ReturnValue<LoginResponse>> Login(LoginInfo UserInfo);
        public Task<ReturnValue<string>> Register(RegisterInfo newUserInfo);
        public Task<ReturnValue<User>> GetClaimedUser(StringValues authHeader);
        public Task<ReturnValue<User>> GetUser(int id);
        public string GenerateJwtSecurityToken(User user);
        public string RefreshJwtSecurityToken();
    }
}