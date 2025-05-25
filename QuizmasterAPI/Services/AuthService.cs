using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Datatypes;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Quizmaster.Services
{
    public class AuthService : IAuthService
    {
        private DatabaseContext _dbContext;
        private IConfiguration _configuration;

        public AuthService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public Task<ReturnValue<User>> GetClaimedUser()
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtSecurityToken(User user)
        {
            var key = _configuration.GetSection("Jwt:Key").Get<string>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var userclaims = new []
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.EMail),
                new Claim("UserID", user.ID.ToString()),
            };

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims:userclaims, expires:DateTime.Now.AddMinutes(120), signingCredentials:credentials);
            var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);
            return writtenToken;
        }

        public async Task<ReturnValue<LoginResponse>> Login(LoginInfo loginInformation)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.EMail == loginInformation.EMail);

            if (user == null)
            {
                return new() {
                    Code = System.Net.HttpStatusCode.Forbidden,
                    IsError = true,
                    Message = "Incorrect username or password."
                };
            }

            bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(loginInformation.Password, user.Password);

            if (!passwordIsCorrect)
            {
                return new() {
                    Code = System.Net.HttpStatusCode.Forbidden,
                    IsError = true,
                    Message = "Incorrect username or password."
                };
            }

            user.Password = "";
            var token = GenerateJwtSecurityToken(user);

            return new() {
                Code = System.Net.HttpStatusCode.OK,
                IsError = false,
                Value = new LoginResponse(user, token)
            };
        }

        public string RefreshJwtSecurityToken()
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValue<string>> Register(RegisterInfo newUserInfo)
        {
            User? oldUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.EMail == newUserInfo.EMail);

            if (oldUser != null)
            {
                return new() {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "User already exists."
                };
            }
            
            User newUser = new ()
            {
                EMail = newUserInfo.EMail,
                Username = newUserInfo.Username,
                Password = "",
                Level = 1,
                Experience = 0,
                ProfilePicture = ""
             };

            BCrypt.Net.BCrypt.GenerateSalt();
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUserInfo.Password);

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return new() {
                Code = System.Net.HttpStatusCode.OK,
                IsError = false,
                Value = "User successfully registered!"
            };
        }
    }
}