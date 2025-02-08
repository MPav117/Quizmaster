using System.IdentityModel.Tokens.Jwt;

public namespace Quizmaster.Interfaces
{
    public interface IAuthService
    {
        public Task Login();
        public Task Register();
        public Task<User> GetClaimedUser();
        public Task<JwtSecurityToken> GetJwtSecurityToken();
        public Task<JwtSecurityToken> RefreshJwtSecurityToken();
    }
}