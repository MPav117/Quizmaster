using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Quizmaster.Datatypes;
using Quizmaster.Models;

namespace Quizmaster.Interfaces
{
    public interface IAuthService
    {
        public Task<ReturnValue<JwtSecurityToken>> Login();
        public Task<ReturnValue<string>> Register(RegisterInfo newUserInfo);
        public Task<ReturnValue<User>> GetClaimedUser();
        public Task<ReturnValue<JwtSecurityToken>> GetJwtSecurityToken();
        public Task<ReturnValue<JwtSecurityToken>> RefreshJwtSecurityToken();
    }
}