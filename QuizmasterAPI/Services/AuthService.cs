using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Datatypes;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Quizmaster.Services
{
    public class AuthService : IAuthService
    {
        private DatabaseContext _dbContext;
        private IPasswordHasher<User> _passwordHasher;
        private IConfiguration _configuration;

        public AuthService(DatabaseContext dbContext, PasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<ReturnValue<User>> GetClaimedUser(StringValues authHeader)
        {
            User? claimedUser = null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = authHeader.ToString();
            jwtToken = jwtToken.Replace("Bearer ", string.Empty);

            if(authHeader.Count > 0)
            {   
                var jsonToken = handler.ReadJwtToken(jwtToken);

                var claim = jsonToken.Claims.First(e => e.Type == "UserID");
                claimedUser = await _dbContext.Users.FindAsync(Int32.Parse(claim.Value));
            }

            if(claimedUser == null) 
            {
                return new() {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    IsError = true,
                    Value = claimedUser
                };
            }
            else
            {
                return new() {
                    Code = System.Net.HttpStatusCode.OK,
                    IsError = false,
                    Value = claimedUser
                };
            }
        }

        public JwtSecurityToken GetJwtSecurityToken(User user)
        {
            var key = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var userClaims = new [] 
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.EMail),
                new Claim("UserID", user.ID.ToString()),
            };
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims:userClaims, expires:DateTime.Now.AddMinutes(180), signingCredentials:credentials);

            return token;
        }

        public async Task<ReturnValue<LoginReturn?>> Login(LoginInfo newLoginInfo)
        {
            // Checking if user exists and validating password
            // TODO: IMPLEMENT PASSWORD HASHING!
            User? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.EMail == newLoginInfo.EMail);
            if(user == null || user.Password != newLoginInfo.password) {
                return new() {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    IsError = true,
                    Value = null
                };
            }

            var token = GetJwtSecurityToken(user);
            user.Password = "";
            
            LoginReturn returnValue = new LoginReturn {
                user = user,
                token = token
            };

            return new() {
                Code = System.Net.HttpStatusCode.OK,
                IsError = false,
                Value = returnValue
            };
        }

        public Task<ReturnValue<JwtSecurityToken>> RefreshJwtSecurityToken()
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValue<string>> Register(RegisterInfo newUserInfo)
        {
            // TODO: IMPLEMENT PASSWORD HASHING!

            User? oldUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.EMail == newUserInfo.email);

            if (oldUser != null)
            {
                return new() {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    IsError = true,
                    ErrorMessage = "User already exists."
                };
            }
            
            User newUser = new ()
            {
                EMail = newUserInfo.email,
                Username = newUserInfo.username,
                Password = newUserInfo.password
            };

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