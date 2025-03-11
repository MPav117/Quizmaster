using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Datatypes;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Quizmaster.Services
{
    public class AuthService : IAuthService
    {
        private DatabaseContext _dbContext;
        private IPasswordHasher<User> _passwordHasher;

        public AuthService(DatabaseContext dbContext, PasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public Task<ReturnValue<User>> GetClaimedUser()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValue<JwtSecurityToken>> GetJwtSecurityToken()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValue<JwtSecurityToken>> Login()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValue<JwtSecurityToken>> RefreshJwtSecurityToken()
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValue<string>> Register(RegisterInfo newUserInfo)
        {
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
                Password = ""
             };

            newUser.Password = _passwordHasher.HashPassword(newUser, newUserInfo.password);

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return new() {
                Code = System.Net.HttpStatusCode.OK,
                IsError = false,
                Value = "User successfully registered!"
            };
        }

        public Task<ReturnValue<string>> Register(string email, string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}